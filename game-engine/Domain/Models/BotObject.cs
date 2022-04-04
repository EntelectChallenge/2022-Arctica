using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Enums;
using Domain.Models.DTOs;
using Domain.Services;

namespace Domain.Models
{
    public class BotObject : GameObject
    {
        public int CurrentTierLevel { get; set; }
        private List<PlayerAction> Actions { get; set; }
        private List<PlayerAction> PendingActions { get; set; }
        public BotMapState Map { get; init; }
        public int Population { get; set; }
        public int AvailableUnits { get; set; }
        public int Score { get; set; }
        public int Placement { get; set; }
        public int Seed { get; init; }
        public int Wood { get; set; }
        public int Food { get; set; }
        public int Stone { get; set; }
        public int Heat { get; set; }

        public BotObject() : base(GameObjectType.PlayerBase)
        {
            PendingActions = new List<PlayerAction>();
            Actions = new List<PlayerAction>();
            Map = new BotMapState();
        }

        public int GetUnitsInAction(ActionType type, int currentTick) => Actions
            .Where(x => x.ActionType == type && x.StartTick <= currentTick).Sum(x => x.NumberOfUnits);

        public int GetUnitsTravelling(int currentTick) =>
            Actions.Where(x => x.StartTick > currentTick).Sum(x => x.NumberOfUnits);

        public int GetPopulation()
        {
            return Population;
        }

        public string GetBotMapState()
        {
            return Map.ToString();
        }

        public void AddAction(PlayerAction playerAction, ResourceNode resourceNode)
        {
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
                    var availableSpace = resourceNode.MaxUnits - resourceNode.CurrentUnits;
                    var unitsToWork = playerAction.NumberOfUnits >= availableSpace
                        ? availableSpace
                        : playerAction.NumberOfUnits;

                    if (unitsToWork <= 0) return;
                    
                    playerAction.NumberOfUnits = unitsToWork;
                    resourceNode.CurrentUnits += unitsToWork;
                    break;
                case ActionType.Error:
                default:
                    return;
            }
            AvailableUnits -= playerAction.NumberOfUnits;
            PendingActions.Add(playerAction);

        }

        public List<PlayerAction> GetActions()
        {
            Actions.AddRange(PendingActions);
            PendingActions.Clear();
            return Actions;
        }

        public void RemoveAction(PlayerAction playerAction)
        {
            AvailableUnits += playerAction.NumberOfUnits;
            Actions.Remove(playerAction);
        }

        public BotDto ToStateObject(GameState gameState) => new BotDto
        {
            Id = Id,
            CurrentTierLevel = CurrentTierLevel,
            Tick = gameState.World.CurrentTick,
            Map = Map,
            AvailableUnits = AvailableUnits,
            Population = GetPopulation(),
            BaseLocation = Position,
            Seed = Seed,
            Wood = Wood,
            Food = Food,
            Stone = Stone,
            Heat =  Heat,
            TravellingUnits = GetUnitsTravelling(gameState.World.CurrentTick),
            FarmingUnits = GetUnitsInAction(ActionType.Farm, gameState.World.CurrentTick),
            MiningUnits = GetUnitsInAction(ActionType.Mine, gameState.World.CurrentTick),
            LumberingUnits = GetUnitsInAction(ActionType.Lumber, gameState.World.CurrentTick),
            ScoutingUnits = GetUnitsInAction(ActionType.Scout, gameState.World.CurrentTick),
            // Gold = Gold,  
        };

        public void VisitScoutTower(Guid scoutTowerId, IEnumerable<Guid> scoutTowerInformation)
        {
            if (!Map.ScoutTowers.Contains(scoutTowerId))
            {
                Map.ScoutTowers.Add(scoutTowerId);
                Map.Nodes.AddRange(scoutTowerInformation);
            }
        }
    }
}