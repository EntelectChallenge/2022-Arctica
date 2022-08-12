using System.Collections.Generic;
using System.Linq;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Services;

namespace Engine.Handlers.Actions;

public class OccupyLandActionHandler : IActionHandler
{
    private readonly IWorldStateService worldStateService;
    private readonly TerritoryService territoryService;
    private readonly ICalculationService calculationService;
    private readonly EngineConfig engineConfig;

    public OccupyLandActionHandler(IWorldStateService worldStateService, TerritoryService territoryService, IConfigurationService engineConfig,
        ICalculationService calculationService)
    {
        this.worldStateService = worldStateService;
        this.territoryService = territoryService;
        this.calculationService = calculationService;
        this.engineConfig = engineConfig.Value;
    }

    public bool IsApplicable(ActionType type)
    {
        return type == ActionType.OccupyLand;
    }

    public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
    {
        foreach (var action in playerActions)
        {
            /*
             * The basic premise of this is to allow competition over territory
             * Each village can send units to a occupy a piece of land.
             * The bot with the most units on that land at the end of each day claims the territory.
             * Units are locked away while this is happening unless a village opts to remove their units with the LeaveLand action
             */
            territoryService.AddOccupants(node, action.Bot, action.NumberOfUnits);
        }
    }
}