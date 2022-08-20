using Domain.Configs;
using Domain.Enums;
using Domain.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class BotObject
    {
        public Guid BotId { get; set; }
        public int CurrentTierLevel { get; set; }
        private List<PlayerAction> Actions { get; set; }
        private List<PlayerAction> PendingActions { get; set; }
        public List<BuildingObject> Buildings { get; set; }
        public Territory Territory { get; set; }
        public BotMapState Map { get; init; }
        public int Population { get; set; }
        public int AvailableUnits { get; set; }
        public int Score { get; set; }
        public int Placement { get; set; }
        public int Seed { get; init; }

        //Resources:
        public int Wood { get; set; }
        public int Food { get; set; }
        public int Stone { get; set; }
        public int Gold { get; set; }
        public int Heat { get; set; }

        //Config values
        public StatusMultiplier StatusMultiplier { get; set; }
        public BotObject() { }

        public BotObject(Guid botId,
            int startingTierLevel,
            int food,
            int availableUnits,
            int population,
            int seed)
        {
            this.BotId = botId;
            this.CurrentTierLevel = startingTierLevel;
            Actions = new List<PlayerAction>();
            PendingActions = new List<PlayerAction>();
            Map = new BotMapState();
            Population = population;

            AvailableUnits = availableUnits;

            Seed = seed;
            Food = food;

            Territory = new Territory();
            Buildings = new List<BuildingObject>();

            StatusMultiplier = new();
        }

        public Position GetBasePosition()
        {
            return Buildings.First(x => x.Type == BuildingType.Base).Position;
        }

        public void RemoveAvailableNode(Guid availableNodeId)
        {

            Map.AvailableNodes.Remove(availableNodeId);
        }

        //Update status effect object with incomming building buff
        public void AddStatusEffect(BuildingType buildingType, int statusMuliplier)
        {

            switch (buildingType)
            {
                case BuildingType.FarmersGuild:
                    StatusMultiplier.FoodReward += statusMuliplier;
                    break;
                case BuildingType.LumberMill:
                    StatusMultiplier.WoodReward += statusMuliplier;
                    break;
                case BuildingType.Quarry:
                    StatusMultiplier.StoneReward += statusMuliplier;
                    StatusMultiplier.GoldReward += statusMuliplier;
                    break;
            }
        }


        public int GetUnitsInAction(ActionType type, int currentTick) => Actions
            .Where(x => x.ActionType == type && x.StartTick <= currentTick).Sum(x => x.NumberOfUnits);

        public int GetUnitsTravelling(int currentTick) =>
            Actions.Where(x => x.IsTravelling(currentTick)).Sum(x => x.NumberOfUnits);

        public int GetPopulation()
        {
            return Population;
        }

        public string GetBotMapState()
        {
            return Map.ToString();
        }

        public void AddAction(PlayerAction playerAction)
        {
            AvailableUnits -= playerAction.NumberOfUnits;
            lock (PendingActions)
            {
                PendingActions.Add(playerAction);
            }
        }

        public List<PlayerAction> GetActions()
        {
            return Actions;
        }

        public List<PlayerAction> GetNewActions()
        {
            List<PlayerAction> newActions;
            lock (PendingActions)
            {
                newActions = PendingActions.ToList();
                PendingActions.Clear();
                lock (Actions)
                {
                    Actions.AddRange(newActions);
                }
            }
            return newActions;
        }

        public void RemoveAction(PlayerAction playerAction)
        {
            AvailableUnits += playerAction.NumberOfUnits;
            lock (Actions)
            {
                Actions.Remove(playerAction);
            }
        }

        public BotDto ToStateObject(GameState gameState) => new BotDto
        {
            Id = BotId,
            CurrentTierLevel = CurrentTierLevel,
            Tick = gameState.World.CurrentTick,
            Map = Map,
            AvailableUnits = AvailableUnits,
            Population = GetPopulation(),
            BaseLocation = GetBasePosition(),
            Seed = Seed,
            Wood = Wood,
            Food = Food,
            Stone = Stone,
            Gold = Gold,
            Heat = Heat,
            PendingActions = GetPendingActions(),
            Actions = Actions.Select(action => action.ToStateObject()).ToList(),
            Territory = Territory.LandInTerritory.ToList(),
            StatusMultiplier = StatusMultiplier,
            Buildings = Buildings
        };

        private List<PlayerActionDto> GetPendingActions()
        {
            lock (PendingActions)
            {
                return PendingActions.Select(action => action.ToStateObject()).ToList();
            }
        }

        public void UpdateTerritory(Territory territory)
        {
            //TODO: Use this method
            this.Territory = territory;
        }

        public void VisitScoutTower(Guid scoutTowerId, IEnumerable<Guid> scoutTowerInformation)
        {
            if (!Map.ScoutTowers.Contains(scoutTowerId))
            {
                Map.ScoutTowers.Add(scoutTowerId);
                Map.Nodes.AddRange(scoutTowerInformation);
            }
        }

        public void AddAvailableNodeIds(IEnumerable<Guid> availableNodes)
        {
            if (Map.AvailableNodes.Count == 0)
            {
                Map.AvailableNodes.AddRange(availableNodes);

                return;
            }

            var excludedAvailableNodes = availableNodes.Where(an => !Map.AvailableNodes.Contains(an));

            Console.WriteLine("Nodes available in AddAvailableNode");
            Console.WriteLine(JsonConvert.SerializeObject(excludedAvailableNodes, Formatting.Indented));

            Map.AvailableNodes.AddRange(excludedAvailableNodes);

        }
    }
}
