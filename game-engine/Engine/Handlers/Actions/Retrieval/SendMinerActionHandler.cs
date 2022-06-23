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
    public class SendMinerActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public SendMinerActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.calculationService = calculationService;
            this.engineConfig = engineConfig.Value;
        }

        public bool IsApplicable(ActionType type) => type == ActionType.Mine;

        public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
        {

            var resourceNode = (ResourceNode)node;
            Logger.LogInfo("Miner Action Handler", "Processing Miner Completed Actions");
            // update to use new numbers
            var totalAmountExtracted = calculationService.CalculateTotalAmountExtracted(resourceNode, playerActions);

            var calculatedTotalAmount =
                totalAmountExtracted < resourceNode.Amount ? totalAmountExtracted : resourceNode.Amount;


            var totalUnitsAtResource = playerActions.Sum(x => x.NumberOfUnits);

            foreach (var playerAction in playerActions)
            {
                var botPopulationTier = calculationService.GetBotPopulationTier(playerAction.Bot);

                double distributionFactor =
                    Convert.ToDouble(calculatedTotalAmount) / Convert.ToDouble(totalUnitsAtResource);
                var resourceDistributed = (int)Math.Round(playerAction.NumberOfUnits * distributionFactor);


                if (resourceNode.Type == ResourceType.Stone)
                {
                    var maxResourceDistributed = botPopulationTier.TierMaxResources.Stone - playerAction.Bot.Stone;
                    resourceDistributed = resourceDistributed.NeverMoreThan(maxResourceDistributed);

                    Logger.LogInfo("Miner Action Handler",
                        $"Bot {playerAction.Bot.BotId} received {resourceDistributed} amount of gold");

                    playerAction.Bot.Stone += resourceDistributed;
                }
                else
                {
                    var maxResourceDistributed = botPopulationTier.TierMaxResources.Gold - playerAction.Bot.Gold;
                    resourceDistributed = resourceDistributed.NeverMoreThan(maxResourceDistributed);

                    Logger.LogInfo("Miner Action Handler",
                        $"Bot {playerAction.Bot.BotId} received {resourceDistributed} amount of gold");

                    playerAction.Bot.Gold += resourceDistributed;
                }

                resourceNode.Amount -= resourceDistributed;
                resourceNode.CurrentUnits -= playerAction.NumberOfUnits;
            }
        }
    }
}