using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
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

        public void ProcessActionComplete(ResourceNode resourceNode, List<PlayerAction> playerActions)
        {
            Logger.LogInfo("Farm Action Handler", "Processing Farm Completed Actions");
            var totalAmountExtracted = calculationService.CalculateTotalAmountExtracted(resourceNode, playerActions);

            var calculatedTotalAmount =
                totalAmountExtracted < resourceNode.Amount ? totalAmountExtracted : resourceNode.Amount;

            var totalUnitsAtResource = playerActions.Sum(x => x.NumberOfUnits);

            foreach (var playerAction in playerActions)
            {
                double distributionFactor = calculationService.CalculateDistributionFactor(calculatedTotalAmount, totalUnitsAtResource);
                var foodDistributed = (int) Math.Round(playerAction.NumberOfUnits * distributionFactor);
                Logger.LogInfo("Farm Action Handler", $"Bot {playerAction.Bot.Id} received {foodDistributed} amount of food");
                playerAction.Bot.Food += foodDistributed;
                resourceNode.Amount -= foodDistributed;
                resourceNode.CurrentUnits -= playerAction.NumberOfUnits;
            }
        }
    }
}