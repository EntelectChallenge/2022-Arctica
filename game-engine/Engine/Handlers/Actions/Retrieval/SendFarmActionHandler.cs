using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Engine.Extensions;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Models;
using Engine.Services;

namespace Engine.Handlers.Actions.Retrieval
{
    public class SendFarmActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public SendFarmActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.calculationService = calculationService;
            this.engineConfig = engineConfig.Value;
        }

        public bool IsApplicable(ActionType type) => type == ActionType.Farm;


        public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
        {
            var resourceNode = (ResourceNode)node;

            Logger.LogInfo("Farm Action Handler", "Processing Farm Completed Actions");
            var totalAmountExtracted = calculationService.CalculateTotalAmountExtracted(resourceNode, playerActions);

            var calculatedTotalAmount =
                totalAmountExtracted < resourceNode.Amount ? totalAmountExtracted : resourceNode.Amount;

            var totalUnitsAtResource = playerActions.Sum(x => x.NumberOfUnits);

            foreach (var playerAction in playerActions)
            {
                var botPopulationTier = calculationService.GetBotPopulationTier(playerAction.Bot);

                double distributionFactor =
                    calculationService.CalculateDistributionFactor(calculatedTotalAmount, totalUnitsAtResource);
                var foodDistributed = (int)Math.Round(playerAction.NumberOfUnits * distributionFactor);

                var maxResourceDistributed = botPopulationTier.TierMaxResources.Food - playerAction.Bot.Food;
                foodDistributed = foodDistributed.NeverMoreThan(maxResourceDistributed);

                Logger.LogInfo("Farm Action Handler",
                    $"Bot {playerAction.Bot.BotId} received {foodDistributed} amount of food");
                playerAction.Bot.Food += foodDistributed;
                playerAction.Bot.Food = playerAction.Bot.Food.NeverMoreThan(botPopulationTier.TierMaxResources.Food);

                resourceNode.Amount -= foodDistributed;
                resourceNode.CurrentUnits -= playerAction.NumberOfUnits;
            }
        }
    }
}