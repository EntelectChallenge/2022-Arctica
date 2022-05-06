using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SendLumberActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public SendLumberActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.calculationService = calculationService;
            this.engineConfig = engineConfig.Value;
        }

        public bool IsApplicable(ActionType type) => type == ActionType.Lumber;

        public void ProcessActionComplete(ResourceNode resourceNode, List<PlayerAction> playerActions)
        {
            // TODO: please write unit tests for this
            Logger.LogInfo("Lumber Action Handler", "Processing Lumber Completed Actions");
            var totalAmountExtracted = calculationService.CalculateTotalAmountExtracted(resourceNode, playerActions);

            var calculatedTotalAmount =
                totalAmountExtracted < resourceNode.Amount ? totalAmountExtracted : resourceNode.Amount;

            var totalUnitsAtResource = playerActions.Sum(x => x.NumberOfUnits);


            foreach (var playerAction in playerActions)
            {
                var botPopulationTier = calculationService.GetBotPopulationTier(playerAction.Bot);

                double distributionFactor = calculationService.CalculateDistributionFactor(calculatedTotalAmount, totalUnitsAtResource);
                var woodDistributed = (int) Math.Round(playerAction.NumberOfUnits * distributionFactor);
                
                var maxResourceDistributed = botPopulationTier.TierMaxResources.Wood - playerAction.Bot.Wood;
                woodDistributed = woodDistributed.NeverMoreThan(maxResourceDistributed);
                
                Logger.LogInfo("Lumber Action Handler", $"Bot {playerAction.Bot.Id} received {woodDistributed} amount of wood");
                playerAction.Bot.Wood += woodDistributed;

                resourceNode.Amount -= woodDistributed;
                resourceNode.CurrentUnits -= playerAction.NumberOfUnits;
            }
        }
    }
}