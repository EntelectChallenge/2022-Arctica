using System.Collections.Generic;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
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
            if (action == null)
            {
                Logger.LogInfo("Occupy land Action Handler - ProcessActionComplete", "Action is Null");
                return;
            }
            else if (node == null)
            {
                Logger.LogInfo("Occupy land Action Handler - ProcessActionComplete", "Node is Null!");
                return;
            }

            Logger.LogInfo("Occupy land Action Handler - ProcessActionComplete", "Processing occupy land Completed Actions");

            /*
             * The basic premise of this is to allow competition over territory
             * Each village can send units to a occupy a piece of land.
             * The bot with the most units on that land at the end of each day claims the territory.
             * Units are locked away while this is happening unless a village opts to remove their units with the LeaveLand action
             */
            territoryService.AddOccupants(node, action.Bot, action.NumberOfUnits);
            action.NumberOfUnits = 0; // setting this to zero so that the units aren't given back to the bot when the action finishes processing
        }
    }
}