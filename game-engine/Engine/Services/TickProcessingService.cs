using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Services;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Models;

namespace Engine.Services
{
    public class TickProcessingService : ITickProcessingService
    {
        private readonly IWorldStateService worldStateService;
        private StopWatchLogger stoplog;

        public TickProcessingService(
            IWorldStateService worldStateService)
        {
            this.worldStateService = worldStateService;
            stoplog = new StopWatchLogger();
        }

        public void SimulateTick()
        {
            Logger.LogDebug("TPS", "Start of tick");
            if (worldStateService.GetPlayerCount() <= 1)
            {
                return;
            }

            var simulationStep = 0;
        }
    }
}