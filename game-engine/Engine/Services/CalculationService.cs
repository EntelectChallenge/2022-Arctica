using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Domain.Services;
using Engine.Interfaces;
using Engine.Models;
using Engine.Extensions;

namespace Engine.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly EngineConfig engineConfig;
        private readonly Random randomGenerator;

        public CalculationService(IConfigurationService engineConfig)
        {
            this.engineConfig = engineConfig.Value;
            this.randomGenerator = new Random(this.engineConfig.WorldSeed);
        }

        private int GetFoodConsumption(BotObject bot)
        {
            return (int)Math.Ceiling(bot.GetPopulation() * engineConfig.UnitConsumptionRatio.Food);
        }

        private int GetWoodConsumption(BotObject bot)
        {
            // TODO: Add the heat consumption stuff here
            return (int)Math.Ceiling(engineConfig.UnitConsumptionRatio.Wood);
        }

        private int GetStoneConsumption(BotObject bot)
        {
            return (int)Math.Ceiling(engineConfig.UnitConsumptionRatio.Stone);
        }

        private int GetHeatConsumption(BotObject bot)
        {
            return (int)Math.Ceiling(bot.GetPopulation() * engineConfig.UnitConsumptionRatio.Heat);
        }

        public int CalculateFoodUpkeep(BotObject bot)
        {
            return bot.Food >= GetFoodConsumption(bot)
                ? GetFoodConsumption(bot)
                : bot.Food;
        }

        public int CalculateWoodUpkeep(BotObject bot)
        {
            return bot.Wood >= GetWoodConsumption(bot)
                ? GetWoodConsumption(bot)
                : bot.Wood;
        }

        public int CalculateStoneUpkeep(BotObject bot)
        {
            return bot.Stone >= GetStoneConsumption(bot)
                ? GetStoneConsumption(bot)
                : bot.Stone;
        }

        public int CalculateHeatUpkeep(BotObject bot)
        {
            return bot.Heat >= GetHeatConsumption(bot)
                ? GetHeatConsumption(bot)
                : bot.Heat;
        }


        public int GetPopulationChange(BotObject bot)
        {
            var populationTier = GetBotPopulationTier(bot);

            var heatSurplus = (bot.Heat - GetHeatConsumption(bot)) * engineConfig.ResourceImportance.Heat;
            var foodSurplus = (bot.Food - GetFoodConsumption(bot)) * engineConfig.ResourceImportance.Food;

            var minResourceSurplus = (double)Math.Min(heatSurplus, foodSurplus);

            var populationRangeMin = bot.Population * -0.5;
            var populationRangeMax = bot.Population * 0.5;
            var populationChangeMin = populationTier.PopulationChangeFactorRange[0];
            var populationChangeMax = populationTier.PopulationChangeFactorRange[1];
            Logger.LogInfo("Calculation Service", $"Min Resource surplus {minResourceSurplus}");

            minResourceSurplus = minResourceSurplus.NeverLessThan(populationRangeMin).NeverMoreThan(populationRangeMax);

            var populationChangeFactor =
                (minResourceSurplus - populationRangeMin) * (populationChangeMax - populationChangeMin) /
                (populationRangeMax - populationRangeMin) + populationChangeMin;

            var populationChange = Math.Ceiling(bot.Population * populationChangeFactor);

            Logger.LogInfo("Calculation Service", $"Population change factor {populationChangeFactor}");
            Logger.LogInfo("Calculation Service", $"Population change {populationChange}");

            // Calculate the max population for the current tier
            if (populationTier.MaxPopulation != -1)
            {
                if (bot.GetPopulation() + populationChange > populationTier.MaxPopulation)
                {
                    return populationTier.MaxPopulation - bot.GetPopulation();
                }
            }

            return (int)populationChange;
        }

        public PopulationTier GetBotPopulationTier(BotObject bot)
        {
            return engineConfig.PopulationTiers.Single(tier => tier.Level == bot.CurrentTierLevel);
        }

        public double CalculateDistributionFactor(int calculatedTotalAmount, int totalUnitsAtResource)
        {
            if (calculatedTotalAmount <= 0 || totalUnitsAtResource <= 0)
                return 0;
            return Convert.ToDouble(calculatedTotalAmount) / Convert.ToDouble(totalUnitsAtResource);
        }

        public int CalculateAmountUsed(PlayerAction playerAction)
        {
            if (playerAction.ActionType == ActionType.StartCampfire)
            {
                var resourceConsumption = randomGenerator.Next(
                    engineConfig.ResourceGenerationConfig.Campfire.ResourceConsumption[ResourceType.Wood][0],
                    engineConfig.ResourceGenerationConfig.Campfire.ResourceConsumption[ResourceType.Wood][1]);
                return playerAction.NumberOfUnits * resourceConsumption;
            }

            return 0;
        }

        public int GetTravelTime(Node targetNode, BotObject bot)
        {
            var distance = CalculateDistance(targetNode.Position, bot.GetBasePosition());

            // What should multiplier depend on? - weather, group size, action type, etc.
            // move to config 
            // Can be altered with affects from buildings

            var travelRate = 1;

            return (int)Math.Round(distance * travelRate);
        }

        public int GetWorkTime(ResourceNode node, PlayerAction action)
        {
            // Calculate the work time for the action: default to the nodes work time
            // Can be altered with affects from buildings
            return node.WorkTime;
        }

        public int GetWorkTime(AvailableNode node, PlayerAction action)
        {
            // Calculate the work time for the action: default to the nodes work time

            //TODO: reduce time here, if there are more units working OR restrict the number of units the can build at a time

            return engineConfig.Buildings.FirstOrDefault(buildingConfig => (short)action.ActionType == (short)buildingConfig.BuildingType).BuildTime;
        }

        public int CalculateAmountExtracted(ResourceNode node, PlayerAction action)
        {
            // These can be altered with affects from buildings
            if (action.ActionType == ActionType.StartCampfire)
            {
                var reward = randomGenerator.Next(
                    engineConfig.ResourceGenerationConfig.Campfire.RewardRange[0],
                    engineConfig.ResourceGenerationConfig.Campfire.RewardRange[1]);
                return action.NumberOfUnits * reward;
            }

            var statusMultiplier = action.Bot.StatusMultiplier;

            return node.Type switch
            {
                ResourceType.Food => action.NumberOfUnits * ApplyStatusMuliplier(node, action, statusMultiplier.FoodReward),
                ResourceType.Wood => action.NumberOfUnits * ApplyStatusMuliplier(node, action, statusMultiplier.WoodReward),
                ResourceType.Stone => action.NumberOfUnits * ApplyStatusMuliplier(node, action, statusMultiplier.StoneReward),
                ResourceType.Gold => action.NumberOfUnits * ApplyStatusMuliplier(node, action, statusMultiplier.GoldReward),
                _ => 0
            };
        }

        private int ApplyStatusMuliplier(ResourceNode node, PlayerAction action, int statusMultipler)
        {
            var isIntTerritory = IsInTerritory(node, action);

            return node.Reward + (isIntTerritory ? statusMultipler : 0);
        }

        private bool IsInTerritory(ResourceNode resourceNode, PlayerAction action)
        {
            return action.Bot.Territory.Contains(resourceNode.Position);
        }


        public int CalculateTotalAmountExtracted(ResourceNode resourceNode, List<PlayerAction> playerActions)
        {
            return playerActions.Sum(x => CalculateAmountExtracted(resourceNode, x));
        }

        private double CalculateDistance(Position a, Position b)
        {
            var deltaX = a.X - b.X;
            var deltaY = a.Y - b.Y;
            var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);
            return Math.Sqrt(distanceSquared);
        }


        public int GetScore(BotObject bot)
        {
            var score =
                bot.GetPopulation() * engineConfig.ResourceScoreMultiplier.Population +
                bot.Wood * engineConfig.ResourceScoreMultiplier.Wood +
                bot.Stone * engineConfig.ResourceScoreMultiplier.Stone +
                bot.Food * engineConfig.ResourceScoreMultiplier.Food +
                bot.Buildings.Sum(building => building.ScoreMultiplier);
            return score;
        }
    }
}