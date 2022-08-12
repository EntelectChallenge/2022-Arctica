using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Configs;
using Domain.Models;
using Engine.Interfaces;
using BuildingConfig = Domain.Configs.Building;

namespace Engine.Services;

public class ObjectGenerationService
{
    private readonly IWorldStateService worldStateService;
    private readonly TerritoryService territoryService;
    private readonly EngineConfig engineConfig;

    public ObjectGenerationService(IWorldStateService worldStateService, IConfigurationService configurationService, TerritoryService territoryService)
    {
        this.worldStateService = worldStateService;
        this.territoryService = territoryService;
        engineConfig = configurationService.Value;
    }
    
    public BotObject CreateBotObject(Guid id)
    {
        BuildingConfig buildingConfig = engineConfig.Buildings.FirstOrDefault(x => x.BuildingType.Equals(BuildingType.Base));

        //Change this  buildingConfig.BuildingType
        BuildingObject baseBuilding = new BuildingObject(worldStateService.GetNextBotPosition(), buildingConfig.TerritorySquare, BuildingType.Base, buildingConfig.ScoreMultiplier);

        //Clear position
        // worldStateService.GetPositionsInUse().Remove(baseBuilding.Position); // Removed this because I don't think it is a blocker to placing the base building

        var state = worldStateService.GetState();
        
        var bot = new BotObject
        (
            id,
            0,
            engineConfig.StartingFood,
            engineConfig.StartingUnits,
            engineConfig.StartingUnits,
            engineConfig.Seeds.PlayerSeeds[state.Bots.Count]
        )
        {
            Wood = 50,
            Gold = 35,
            Stone = 50,
            Food = 50,
            Heat = 50
        };

        state.Bots.Add(bot);
        worldStateService.AddAvailableNodes(new List<AvailableNode>{new AvailableNode(baseBuilding.Position)});
        territoryService.AddBuilding(bot, baseBuilding);

        return bot;
    }

}