using System.Collections.Generic;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Services;

namespace Engine.Handlers.Actions;

public class LeaveLandActionHandler : IActionHandler
{
    private readonly IWorldStateService worldStateService;
    private readonly TerritoryService territoryService;
    private readonly ICalculationService calculationService;
    private readonly EngineConfig engineConfig;

    public LeaveLandActionHandler(IWorldStateService worldStateService, TerritoryService territoryService, IConfigurationService engineConfig,
        ICalculationService calculationService)
    {
        this.worldStateService = worldStateService;
        this.territoryService = territoryService;
        this.calculationService = calculationService;
        this.engineConfig = engineConfig.Value;
    }

    public bool IsApplicable(ActionType type)
    {
        return type == ActionType.LeaveLand;
    }

    public void ProcessActionComplete(Node node, List<PlayerAction> playerActions)
    {
        foreach (var action in playerActions)
        {
            territoryService.LeaveLand(action.Bot, action.TargetNodeId);
        }
    }
}