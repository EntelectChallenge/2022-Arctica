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

namespace Engine.Handlers.Actions
{
    public class StartCampfireActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public StartCampfireActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig,
            ICalculationService calculationService)
        {
            this.worldStateService = worldStateService;
            this.calculationService = calculationService;
            this.engineConfig = engineConfig.Value;
        }
        
        public bool IsApplicable(ActionType type) => type == ActionType.StartCampfire;

        public void ProcessActionComplete(ResourceNode resourceNode, List<PlayerAction> playerActions)
        {
            Logger.LogInfo("Burn Wood Action Handler", "Processing Burn Wood Completed Actions");
            foreach (var playerAction in playerActions)
            {
                var woodUsed = calculationService.CalculateAmountUsed(playerAction);
                var heatExtracted = calculationService.CalculateAmountExtracted(resourceNode, playerAction);
                if (playerAction.Bot.Wood - woodUsed >= 0)
                {
                    playerAction.Bot.Wood -= woodUsed;
                    playerAction.Bot.Heat += heatExtracted;
                }
            }
        }
    }
}