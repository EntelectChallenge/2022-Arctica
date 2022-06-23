using System;
using System.Collections.Generic;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Models;
using Engine.Services;

namespace Engine.Handlers.Actions.Retrieval
{
    public class SendScoutActionHandler : IActionHandler
    {
        private readonly IWorldStateService worldStateService;
        private readonly EngineConfig engineConfig;

        public SendScoutActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig)
        {
            this.worldStateService = worldStateService;
            this.engineConfig = engineConfig.Value;
        }

        public bool IsApplicable(ActionType type) => type == ActionType.Scout;

        public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
        {
            Logger.LogInfo("Scout Action Handler", "Processing Scout Completed Actions");
            foreach (var playerAction in playerActions)
            {
                playerAction.Bot.VisitScoutTower(playerAction.TargetNodeId,
                    worldStateService.GetScoutTowerInformation(playerAction.TargetNodeId));
            }
        }
    }
}