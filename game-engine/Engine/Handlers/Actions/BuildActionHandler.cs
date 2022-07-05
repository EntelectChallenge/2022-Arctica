using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Domain.Configs;
using Engine.Services;
using System.Collections.Generic;
using System.Linq;
using Building = Domain.Models.BuildingObject;
using BuildingConfig = Domain.Configs.Building;

namespace Engine.Handlers.Actions
{
    internal class BuildActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public BuildActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.calculationService = calculationService;
            this.engineConfig = engineConfig.Value;
        }

        public bool IsApplicable(ActionType type)
        {
            switch (type)
            {
                case ActionType.Quarry: return true;
                case ActionType.LumberMill: return true;
                case ActionType.FarmersGuild: return true;
            }
            return false;
        }


        public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
        {
            foreach (var playerAction in playerActions)
            {
                var bot = playerAction.Bot;

                if (!worldStateService.GetState().World.Map.AvailableNodes.Any(an => playerAction.TargetNodeId == an.Id))
                {
                    Logger.LogDebug("Build Action Handler", $"Invalid action {playerAction.TargetNodeId} has already been taken");
                    return;
                }


                //TODO: add isInTerrotry methods to world state service ...




                Logger.LogInfo("Build Action Handler", "Processing Build Completed Action");

                Position position = worldStateService.ResolveNodePosition(playerAction);

                BuildingConfig buildingConfig = engineConfig.Buildings.FirstOrDefault(x => (short)x.BuildingType == (short)playerAction.ActionType);

                BuildingType buildingType = (BuildingType)playerAction.ActionType;

                BuildingObject newBuilding = new Building(
                    position,
                    buildingConfig.TerritorySquare,
                    buildingType,
                    buildingConfig.ScoreMultiplier);


                //TODO: add to calculation service? 

                var w = bot.Wood;
                var g = bot.Gold;
                var s = bot.Stone;

                ApplyBuildingCosts(bot, buildingConfig, buildingType);

                if (IsNegative(bot.Wood) ||
                   IsNegative(bot.Gold) ||
                   IsNegative(bot.Stone))
                {
                    bot.Wood = w;
                    bot.Gold = g;
                    bot.Stone = s;
                    return;
                }

                //Add the increase cost logic here
                bot.UpdateBuildingList(newBuilding, worldStateService.GetClaimedTerritory());

                bot.RemoveAvaialableNode(playerAction.TargetNodeId);


                worldStateService.UpdateTerritory(bot, bot.Territory.PositionsInTerritory.ToList());
                worldStateService.AddPositionInUse(position);

                // List<AvailableNode> validAvialableNodes = worldStateService.ValidateTerritoryNode(bot);


                IList<AvailableNode> validAvailableNodes = worldStateService.ValidateAvaialbleNodes(bot);

                worldStateService.AddAvailableNodes(validAvailableNodes.ToList());
                bot.AddAvailableNodeIds(validAvailableNodes.Select(node => node.Id));


                //TODO: check if the avaialable node is still available  

                bot.AddStatusEffect(buildingType, buildingConfig.StatusEffectMultiplier);
                worldStateService.RemoveAvailableNode(playerAction.TargetNodeId);
            }
        }


        //TODO: move to bot object?
        private static void ApplyBuildingCosts(BotObject bot, BuildingConfig buildingConfig, BuildingType buildingType)
        {
            var buildingCount = GetBuildingsByType(bot, buildingType);

            var woodCost = GetBuildingCost(buildingCount, buildingConfig.Cost.Wood);
            var goldCost = GetBuildingCost(buildingCount, buildingConfig.Cost.Gold);
            var stoneCost = GetBuildingCost(buildingCount, buildingConfig.Cost.Stone);

            bot.Wood -= buildingConfig.Cost.Wood + woodCost;
            bot.Gold -= buildingConfig.Cost.Gold + goldCost;
            bot.Stone -= buildingConfig.Cost.Stone + stoneCost;
        }

        private static int GetBuildingsByType(BotObject bot, BuildingType buildingType) => bot.Buildings.Count(building => (short)building.Type == (short)buildingType);


        private static int GetBuildingCost(int numberOfBuildingsPerType, int cost) => (numberOfBuildingsPerType * cost) / 2;

        private static bool IsNegative(int amount)
        {
            return amount < 0;

        }
    }
}
