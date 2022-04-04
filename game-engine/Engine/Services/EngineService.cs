using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Domain.Enums;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Services;
using Engine.Interfaces;
using Engine.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Engine.Services
{
    public class EngineService : IEngineService
    {
        private readonly EngineConfig engineConfig;
        private readonly IActionService actionService;
        private readonly IWorldStateService worldStateService;
        private readonly ITickProcessingService tickProcessingService;
        private readonly ICalculationService calculationService;
        private HubConnection hubConnection;
        public int TickAcked { get; set; }
        public bool HasWinner { get; set; }
        public bool PendingStart { get; set; }
        public bool GameStarted { get; set; }

        public EngineService(
            IWorldStateService worldStateService,
            IActionService actionService,
            IConfigurationService engineConfig,
            ITickProcessingService tickProcessingService,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.actionService = actionService;
            this.tickProcessingService = tickProcessingService;
            this.engineConfig = engineConfig.Value;
            this.calculationService = calculationService;
        }

        public HubConnection SetHubConnection(ref HubConnection connection) => hubConnection = connection;

        public async Task GameRunLoop()
        {
            worldStateService.Initialize();

            // Logger.LogData(worldStateService.GetWorldState());

            var stopwatch = Stopwatch.StartNew();
            var stop2 = Stopwatch.StartNew();
            do
            {
                if (hubConnection.State != HubConnectionState.Connected)
                {
                    break;
                }

                if (!GameStarted)
                {
                    if (!PendingStart)
                    {
                        Logger.LogInfo("Core", "Waiting for all bots to connect");
                    }

                    Thread.Sleep(1000);
                    continue;
                }

                //TODO: Sleep was added here to give bots time to send commands - Make this better! 
                //Thread.Sleep(500);
                stop2.Restart();


                await ProcessGameTick();

                Logger.LogDebug("RunLoop", $"Processing tick took {stop2.ElapsedMilliseconds}ms");

                stop2.Restart();
                var gameStateDto = worldStateService.GetPublishedState();
                var gameState = worldStateService.GetState();

                foreach (var bot in worldStateService.GetPlayerBots())
                {
                    var botGameState = new GameStateDto();
                    botGameState.BotId = bot.Id;
                    botGameState.PopulationTiers = engineConfig.PopulationTiers.ToList();
                    botGameState.World = new World()
                    {
                        CurrentTick = gameState.World.CurrentTick,
                        Size = gameState.World.Size,
                        Map = new Map()
                        {
                            Nodes = gameState.World.Map.Nodes.Where(node => bot.Map.Nodes.Contains(node.Id))
                                .ToList(),
                            ScoutTowers = gameState.World.Map.ScoutTowers.Select(scoutTower => new ScoutTower()
                            {
                                Id = scoutTower.Id,
                                Position = scoutTower.Position,
                                GameObjectType = scoutTower.GameObjectType
                            }).ToList()
                        }
                    };
                    botGameState.Bots = gameState.Bots.Select(otherBot =>
                    {
                        if (bot.Id == otherBot.Id)
                        {
                            return bot.ToStateObject(gameState);
                        }

                        Console.WriteLine("new botDto");
                        return new BotDto()
                        {                    
                            Id = otherBot.Id,
                            Population = otherBot.GetPopulation(),
                            Food = otherBot.Food,
                            Stone = otherBot.Stone,
                            Wood = otherBot.Wood,
                            Heat = otherBot.Heat,
                            CurrentTierLevel = otherBot.CurrentTierLevel
                        };
                    }).ToList();


                    await hubConnection.InvokeAsync("PublishBotState", botGameState);
                }

                Logger.LogDebug("RunLoop", $"Published bot game states, Time: {stop2.ElapsedMilliseconds}");

                await hubConnection.InvokeAsync("PublishGameState", gameStateDto);
                // Logger.LogData(gameStateDto.ToString());

                Logger.LogDebug("RunLoop", $"Published game state, Time: {stop2.ElapsedMilliseconds}");

                Logger.LogDebug("RunLoop", "Waiting for Tick Ack");
                stop2.Restart();


                // Wait until the game runner has processed the current tick
                while (TickAcked != worldStateService.GetCurrentTick())
                {
                }


                Logger.LogDebug("RunLoop", $"TickAck matches current tick, Time: {stop2.ElapsedMilliseconds}");

                if (stopwatch.ElapsedMilliseconds < engineConfig.TickRate)
                {
                    var delay = (int) (engineConfig.TickRate - stopwatch.ElapsedMilliseconds);
                    if (delay > 0)
                    {
                        Thread.Sleep(delay);
                    }
                }

                Logger.LogInfo("TIMER", $"Game Loop Time: {stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Restart();
            } while (!HasWinner &&
                     hubConnection.State == HubConnectionState.Connected);

            if (!HasWinner &&
                hubConnection.State != HubConnectionState.Connected)
            {
                Logger.LogError("RunLoop", "Runner disconnected before a winner was found");
                throw new InvalidOperationException("Runner disconnected before a winner was found");
            }

            await hubConnection.InvokeAsync("GameComplete", worldStateService.GenerateGameCompletePayload());
        }

        private async Task ProcessGameTick()
        {
            IList<BotObject> bots = worldStateService.GetPlayerBots();

            Logger.LogInfo("RunLoop", "======================================================================");
            Logger.LogInfo("RunLoop", "Tick: " + worldStateService.GetCurrentTick());

            SimulateTickForBots(bots);
            // TODO: maybe refactor the below into a method for better readability
            // =======================
            var renewableNodes = worldStateService.GetState().World.Map.Nodes
                .Where(node => node.RegenerationRate != null);
            foreach (var node in renewableNodes)
            {
                node.CurrentRegenTick++;
                if (node.CurrentRegenTick >= node.RegenerationRate.Ticks)
                {
                    node.Amount += node.RegenerationRate.Amount;
                    if (node.Amount > node.MaxResourceAmount)
                    {
                        node.Amount = node.MaxResourceAmount;
                    }
                    node.CurrentRegenTick = 0;
                }
            }
            // =======================

            if (worldStateService.GetCurrentTick() % engineConfig.ProcessTick == 0 &&
                worldStateService.GetCurrentTick() != 0)
            {
                // Process the interday processes for each bots
                Logger.LogInfo("RunLoop", "End of day: Updating population size");
                foreach (var bot in bots)
                {
                    bot.Food -= calculationService.CalculateFoodUpkeep(bot);
                    bot.Wood -= calculationService.CalculateWoodUpkeep(bot);
                    bot.Stone -= calculationService.CalculateStoneUpkeep(bot);
                    bot.Heat -= calculationService.CalculateHeatUpkeep(bot);

                    var populationChange = calculationService.GetPopulationChange(bot);
                    Logger.LogInfo("RunLoop", $"Population Delta: {populationChange}");

                    var oldAvailableUnits = bot.AvailableUnits;
                    // TODO: refactor the below into a population change method on bot
                    bot.AvailableUnits += populationChange;
                    bot.Population += populationChange;

                    // Check if they have reached a new tier
                    try
                    {
                        var currentPopulationTier = calculationService.GetBotPopulationTier(bot);
                        var nextTier =
                            engineConfig.PopulationTiers.SingleOrDefault(tier =>
                                tier.Level == bot.CurrentTierLevel + 1);
                        if (nextTier != default)
                        {
                            if (
                                bot.Population >= currentPopulationTier.MaxPopulation &&
                                bot.Food >= nextTier.TierResourceConstraints.Food &&
                                bot.Stone >= nextTier.TierResourceConstraints.Stone &&
                                bot.Wood >= nextTier.TierResourceConstraints.Wood
                            )
                            {
                                bot.CurrentTierLevel++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Tier", e.Message);
                    }


                    Logger.LogInfo("Population Changes",
                        $"{oldAvailableUnits} + {populationChange} = {bot.AvailableUnits}");
                }
            }

            worldStateService.ApplyAfterTickStateChanges();
            foreach (var bot in bots)
            {
                Logger.LogInfo("Bot Resources", $"BotId: {bot.Id}");
                Logger.LogInfo("Bot Resources", $"Population: {bot.GetPopulation()} AvailableUnits: {bot.AvailableUnits}");
                Logger.LogInfo("Bot Resources", $"Food: {bot.Food} Wood: {bot.Wood} Stone: {bot.Stone} Heat: {bot.Heat}");
            }

            // stoplog.Log("After Tick SC Complete");
            CheckWinConditions();
        }

        public void SimulateTickForBots(IList<BotObject> bots)
        {
            var actionsToComplete = new List<PlayerAction>();
            foreach (var bot in bots)
            {
                var currentActions = bot.GetActions();

                foreach (var playerAction in currentActions)
                {
                    if (playerAction.CheckActionComplete(worldStateService.GetCurrentTick()))
                    {
                        actionsToComplete.Add(playerAction);
                    }
                }
            }

            // We have all completed actions for all bots

            // We group them according to resource

            // Send all actions for each resource to the relevant handle method
            var independentPlayerActions = new List<PlayerAction>();
            var groupedPlayerActions = new Dictionary<ResourceNode, List<PlayerAction>>();

            foreach (var playAction in actionsToComplete)
            {
                Logger.LogInfo("RunLoop", "Available units while processing action: " + playAction.Bot.AvailableUnits);
                if (playAction.ActionType == ActionType.Scout || playAction.ActionType == ActionType.StartCampfire)
                {
                    independentPlayerActions.Add(playAction);
                }
                else
                {
                    Logger.LogInfo("RunLoop", "Target Node:" + playAction.TargetNodeId);
                    // Logger.LogInfo("RunLoop", "World State: " + worldStateService.GetWorldState());
                    var node = worldStateService.GetResourceNode(playAction.TargetNodeId);

                    if (groupedPlayerActions.ContainsKey(node))
                    {
                        groupedPlayerActions[node].Add(playAction);
                    }
                    else
                    {
                        groupedPlayerActions.Add(node, new List<PlayerAction> {playAction});
                    }
                }
            }

            // Handle all individual actions
            foreach (var independentPlayerAction in independentPlayerActions)
            {
                var targetNode = worldStateService.GetResourceNode(independentPlayerAction.TargetNodeId);

                actionService.HandleCompletedPlayerAction(targetNode,
                    new List<PlayerAction> {independentPlayerAction},
                    independentPlayerAction.ActionType);
            }

            foreach (var node in groupedPlayerActions.Keys)
            {
                var type = groupedPlayerActions[node].First();
                actionService.HandleCompletedPlayerAction(node, groupedPlayerActions[node], type.ActionType);
            }


            // Remove all actions
            foreach (var playerAction in actionsToComplete)
            {
                playerAction.Remove();
            }

            tickProcessingService.SimulateTick();
        }

        private void CheckWinConditions()
        {
            if (worldStateService.GetCurrentTick() >= engineConfig.MaxTicks)
            {
                // Calculate each bots score
                foreach (var bot in worldStateService.GetPlayerBots())
                {
                    bot.Score = calculationService.GetScore(bot);
                }

                // Calculate their placement

                var bots = worldStateService.GetPlayerBots().OrderByDescending(bot => bot.Score);

                var winningBot = bots.FirstOrDefault();

                //TODO: does not take ties into account

                HasWinner = true;

                // TODO: Log win condition 
                Logger.LogInfo("WinCondition", $"Match end! Winning Bot: {winningBot.Id}");

                Logger.LogInfo("* * * Scores * * *", "");

                // Lazy indexing sorry
                int placement = 1;

                foreach (var botScore in bots)
                {
                    botScore.Placement = placement;
                    placement++;
                }
            }
        }
    }
}