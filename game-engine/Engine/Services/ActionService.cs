using System;
using System.Collections.Generic;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Models;

namespace Engine.Services
{
    public class ActionService : IActionService
    {
        private readonly IWorldStateService worldStateService;
        private readonly IActionHandlerResolver actionHandlerResolver;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;
        private TerritoryService territoryService;

        public ActionService(IWorldStateService worldStateService,
            IActionHandlerResolver actionHandlerResolver,
            ICalculationService calculationService,
            IConfigurationService configurationService, TerritoryService territoryService)
        {
            this.calculationService = calculationService;
            this.territoryService = territoryService;
            this.worldStateService = worldStateService;
            this.actionHandlerResolver = actionHandlerResolver;
            this.engineConfig = configurationService.Value;
        }

        public PlayerAction GetPlayerActionFromCommand(CommandAction command)
        {
            return new PlayerAction(
                command.Type,
                command.Units,
                command.Id);
        }

        public void PushPlayerAction(Guid botId, CommandAction commandAction)
        {
            var playerAction = GetPlayerActionFromCommand(commandAction);

            var targetBot = worldStateService.GetBotById(botId);
            playerAction.Bot = targetBot;

            if (targetBot == null) return;

            var node = ResolveNode(playerAction);

            // var resourceNode = worldStateService.ResolveNode(playerAction);
            if (node != null && IsValid(playerAction, node))
            {
                var travelTime = calculationService.GetTravelTime(node, targetBot);

                int workTime = GetWorkTimeByActionType(playerAction, node);

                // Todo: does this still work with the resource workTime that has been added?
                playerAction.SetStartAndEndTicks(worldStateService.GetCurrentTick(), travelTime, workTime);

                // Logger.LogInfo("ActionService", $"Bot: {botId}, Issued command: {playerAction.ActionType} in tick {worldStateService.GetCurrentTick()}, with {playerAction.NumberOfUnits} units to start at: {playerAction.StartTick}, and to end at: {playerAction.ExpectedCompletedTick}");

                AddAction(targetBot, playerAction, node);
            }
            else
            {
                Logger.LogInfo("ActionService", "Invalid action received");
            }
        }

        public Node ResolveNode(PlayerAction playerAction)
        {
            switch (playerAction.ActionType)
            {
                case ActionType.Scout:
                    return worldStateService.GetScoutTowerAsResourceNode(playerAction.TargetNodeId);
                case ActionType.StartCampfire:
                    return worldStateService.GetBaseAsResourceNode(playerAction.Bot);
                case ActionType.Mine:
                case ActionType.Farm:
                case ActionType.Lumber:
                    return worldStateService.GetResourceNode(playerAction.TargetNodeId);
                case ActionType.Quarry:
                case ActionType.LumberMill:
                case ActionType.FarmersGuild:
                case ActionType.OutPost:
                case ActionType.Road: 
                    return worldStateService.GetAvailableNode(playerAction.Bot, playerAction.TargetNodeId);
                case ActionType.OccupyLand:
                case ActionType.LeaveLand:
                    return worldStateService.GetNode(playerAction.TargetNodeId);
                case ActionType.Error:
                default:
                    return null;
            }
        }

        private int GetWorkTimeByActionType(PlayerAction playerAction, Node node)
        {
            switch (playerAction.ActionType)
            {
                case ActionType.Mine:
                case ActionType.Farm:
                case ActionType.Lumber:
                    return calculationService.GetWorkTime((ResourceNode) node, playerAction);
                case ActionType.Quarry:
                case ActionType.FarmersGuild:
                case ActionType.LumberMill:
                case ActionType.OutPost:
                case ActionType.Road:
                    return calculationService.GetWorkTime((AvailableNode) node, playerAction);
                case ActionType.OccupyLand:
                case ActionType.LeaveLand:
                case ActionType.Scout:
                case ActionType.StartCampfire:
                    return 0;
                case ActionType.Error:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddAction(BotObject targetBot, PlayerAction playerAction, Node node)
        {
            int availableSpace = 0;
            int unitsToWork = 0;
            switch (playerAction.ActionType)
            {
                case ActionType.Scout:
                    playerAction.NumberOfUnits = playerAction.NumberOfUnits >= 1 ? 1 : playerAction.NumberOfUnits;
                    break;
                case ActionType.StartCampfire:
                    break;
                case ActionType.Mine:
                case ActionType.Farm:
                case ActionType.Lumber:
                    availableSpace = node.MaxUnits - node.CurrentUnits;
                    unitsToWork = playerAction.NumberOfUnits >= availableSpace
                        ? availableSpace
                        : playerAction.NumberOfUnits;

                    if (unitsToWork <= 0) return;

                    playerAction.NumberOfUnits = unitsToWork;
                    // evenly distributing the newly added units in EngineService.DistributeHarvestingActionSlots()
                    // node.CurrentUnits += unitsToWork;
                    break;
                case ActionType.Quarry:
                case ActionType.FarmersGuild:
                case ActionType.LumberMill:
                case ActionType.OutPost:
                case ActionType.Road:
                    //Should we force players to send a sertin number of builders at a time OR will the number of units sent effect the build duration
                    playerAction.NumberOfUnits = playerAction.NumberOfUnits >= 1 ? 1 : playerAction.NumberOfUnits;


                    availableSpace = node.MaxUnits - node.CurrentUnits;
                    unitsToWork = playerAction.NumberOfUnits >= availableSpace
                        ? availableSpace
                        : playerAction.NumberOfUnits;

                    if (unitsToWork <= 0) return;

                    playerAction.NumberOfUnits = unitsToWork;
                    node.CurrentUnits += unitsToWork;
                    break;
                case ActionType.OccupyLand:
                    // don't change the amount of units
                    break;
                case ActionType.LeaveLand:
                    // only take one unit to send the message that the others should leave
                    playerAction.NumberOfUnits = 1;
                    break;
                case ActionType.Error:
                default:
                    return;
            }
            targetBot.AddAction(playerAction);
        }


        public void HandleCompletedPlayerAction(Node node, List<PlayerAction> playerActions,
            ActionType type)
        {
            var handler = actionHandlerResolver.ResolveHandler(type);
            handler.ProcessActionComplete(node, playerActions);
        }

        private bool IsValid(PlayerAction action, Node node)
        {
            var resourceType = node.Type;
            var validUnitAmount = (action.Bot.AvailableUnits >= action.NumberOfUnits) && action.NumberOfUnits > 0;
            var actionTypeAndResourceTypeMatch = ActionTypeMatchesResourceType(action.ActionType, resourceType);
            var resourceRequirementsMet = AreSufficientResourcesAvailable(action);
            var extraValidation = ExtraValidation(action, node);

            return validUnitAmount && actionTypeAndResourceTypeMatch && resourceRequirementsMet && extraValidation;
        }

        private bool ExtraValidation(PlayerAction action, Node node)
        {
            switch (action.ActionType)
            {
                case ActionType.OccupyLand:
                {
                    // The following criteria need to be met:
                    // 1. The node must be part of any bot's territory
                    // 2. The occupy land action requires that either:
                    //      1. the target node shares an edge/vertex with the bot's current territory.
                    //      2. or the target node's land currently has occupants from the bot.

                    var land = territoryService.GetLandByNodeId(action.TargetNodeId);

                    var isNodePartOfAnyBotTerritory = land is not null;
                    if (!isNodePartOfAnyBotTerritory) return false;

                    var isNextToTerritory = territoryService.CheckIfPositionIsNextToBotTerritory(land, action.Bot.BotId);
                    
                    var hasOccupants = territoryService.HasOccupants(action.Bot, land);

                    return isNextToTerritory || hasOccupants;
                }
                case ActionType.LeaveLand:
                {
                    // The following criteria need to be met:
                    // 1. The node must be part of any bot's territory
                    // 2. The leave land action requires that the target node's land currently has occupants from the bot.
                    var land = territoryService.GetLandByNodeId(action.TargetNodeId);

                    var isNodePartOfTerritory = land is not null;
                    if (!isNodePartOfTerritory) return false;

                    var hasOccupants = territoryService.HasOccupants(action.Bot, land);

                    return hasOccupants;
                }
                default:
                    return true;
            }
        }

        public bool ActionTypeMatchesResourceType(ActionType actionType, ResourceType resourceType)
        {
            return actionType switch
            {
                ActionType.Error => false,
                ActionType.Scout => true,
                ActionType.StartCampfire => true,
                ActionType.Quarry => resourceType == ResourceType.Available,
                ActionType.FarmersGuild => resourceType == ResourceType.Available,
                ActionType.LumberMill => resourceType == ResourceType.Available,
                ActionType.OutPost => resourceType == ResourceType.Available, 
                ActionType.Road => resourceType == ResourceType.Available,
                ActionType.Mine => resourceType is ResourceType.Stone or ResourceType.Gold,
                ActionType.Farm => resourceType == ResourceType.Food,
                ActionType.Lumber => resourceType == ResourceType.Wood,
                ActionType.OccupyLand => true,
                ActionType.LeaveLand => true,
                _ => false
            };
        }

        private bool AreSufficientResourcesAvailable(PlayerAction action)
        {
            if (action.ActionType == ActionType.StartCampfire)
            {
                return action.Bot.Wood >= calculationService.CalculateAmountUsed(action);
            }

            return true;
        }

    }
}