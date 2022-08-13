using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Engine.Interfaces;

namespace Engine.Services;

public class TerritoryService
{
    private IWorldStateService worldStateService;
    private readonly ISet<Position> claimedTerritory = new HashSet<Position>(new PositionComparer());
    private Dictionary<Position, Land> LandByPosition = new Dictionary<Position, Land>();
    private Dictionary<Guid, Land> LandByNodeId = new Dictionary<Guid, Land>();

    public TerritoryService(IWorldStateService worldStateService)
    {
        this.worldStateService = worldStateService;
    }
    
    public Guid GetOwnerGuid(Position position)
    {
        if (!LandByPosition.ContainsKey(position)) return Guid.Empty;
        var owner = LandByPosition[position].Owner;
        if (owner == Guid.Empty)
        {
            throw new PositionNotOwnedException {Position = position};
        }

        return owner;
    }

    public Land GetLandByNodeId(Guid landId)
    {
        return LandByNodeId.ContainsKey(landId) ? LandByNodeId[landId] : null;
    }

    public Land GetLandByPosition(Position position)
    {
        return LandByPosition.ContainsKey(position) ? LandByPosition[position] : null;
    }

    private void RemoveLandFromBotTerritory(Land land, BotObject oldOwner)
    {
        oldOwner.Territory.RemoveLand(land);
        land.Owner = Guid.Empty;
        land.GetOccupantsByBotDictionary()[oldOwner.BotId].SetOwnsLand(false);
    }

    private void AddLandToBotTerritory(Land land, BotObject newOwner)
    {
        newOwner.Territory.AddLand(land);
        land.Owner = newOwner.BotId;
        land.GetOccupantsByBotDictionary()[newOwner.BotId].SetOwnsLand(true);
    }

    public void RecalculateTerritories()
    {
        // iterate through all the land that is in competition
        // calculate the territory owner: the bot with the highest unit occupancy
        // update the land objects
        // update the territories on the bot objects
        var bots = worldStateService.GetPlayerBots();
        var territoriesToShift = LandByPosition.Values.Where(land => land.GetOccupantsByBotDictionary().Count > 1);
        foreach (var land in territoriesToShift)
        {
            var dominantBotGuid = GetDominantBot(land);
            if (dominantBotGuid == land.Owner) continue;
            var oldOwner = bots.First(bot => bot.BotId == land.Owner);
            var newOwner = bots.First(bot => bot.BotId == dominantBotGuid);
            ShiftTerritory(oldOwner, newOwner, land);
        }
    }

    private Guid GetDominantBot(Land land)
    {
        var maxPressure = land.GetOccupantsByBotDictionary().Max(pair => pair.Value.Pressure);
        
        var botsWithHighestPressure = land.GetOccupantsByBotDictionary().Where(pair => pair.Value.Pressure == maxPressure).ToList();
        return botsWithHighestPressure.Count == 1 ? botsWithHighestPressure.First().Key : land.Owner;
    }

    private void ShiftTerritory(BotObject oldOwner, BotObject newOwner, Land land)
    {
        RemoveLandFromBotTerritory(land, oldOwner);
        AddLandToBotTerritory(land, newOwner);
    }

    public void AddBuilding(BotObject bot, BuildingObject building)
    {
        var buildingNode = worldStateService.NodeByPosition(building.Position);
        if (buildingNode.GetType() != typeof(AvailableNode)) return;

        bot.Buildings.Add(building);
        RemoveAvailableNodeWhereBuildingIsPlaced(bot, (AvailableNode)buildingNode);
        AddBuildingTerritory(bot, building);
    }

    private void RemoveAvailableNodeWhereBuildingIsPlaced(BotObject bot, AvailableNode availableNode)
    {
        bot.RemoveAvailableNode(availableNode.Id);
        worldStateService.RemoveAvailableNode(availableNode.Id);
    }

    private void AddBuildingTerritory(BotObject bot, BuildingObject building)
    {
        var buildingTerritory = GetPositionsInBuildingRadius(building);
        var newBuildingTerritory = buildingTerritory.Where(position => !claimedTerritory.Contains(position)).ToList();
        
        // creating new available nodes here just before creating the new land since it requires the worldStateService having all the available and resource nodes 
        CreateAvailableNodes(bot, newBuildingTerritory);

        newBuildingTerritory.ForEach(position => CreateNewLand(bot, position));
    }

    private Land? CreateNewLand(BotObject bot, Position position)
    {
        // check if the contained node is null, if it is the position isn't an
        // available node (e.g. a scout tower position)
        var containedNode = worldStateService.NodeByPosition(position);
        if (containedNode == null) {
            return null;
        }

        var land = new Land(position, bot.BotId, containedNode.Id);

        LandByPosition[containedNode.Position] = land;
        LandByNodeId[containedNode.Id] = land;
        AddOccupantsToLand(land, bot, 0);

        bot.Territory.AddLand(land);
        worldStateService.GetScoutTowerByRegion(land).AddTerritoryNode(land);
        claimedTerritory.Add(land);

        return land;
    }

    private List<AvailableNode> CreateAvailableNodes(BotObject bot, List<Position> territoryPositions)
    {
        var positionsInUse = worldStateService.GetPositionsInUse();
        var availablePositionsInTerritory = territoryPositions.Where(position => !positionsInUse.Contains(position)).ToList();
        var availableNodes = availablePositionsInTerritory.Select(position => new AvailableNode(position){MaxUnits = 10 /*Is this necessary??*/}).ToList();
        positionsInUse.UnionWith(availablePositionsInTerritory);

        worldStateService.AddAvailableNodes(availableNodes);
        bot.AddAvailableNodeIds(availableNodes.Select(node => node.Id));

        return availableNodes;
    }

    private IEnumerable<int> GenerateDimensionSubset(int centre, int size)
    {
        var start = centre - size;
        var count = size * 2 + 1;
        return Enumerable.Range(start, count);
    }

    private IEnumerable<Position> GetPositionsInBuildingRadius(BuildingObject building)
    {
        return GeneratePositionSquare(building.Position, building.TerritorySquare);
    }

    private IEnumerable<Position> GeneratePositionSquare(Position centre, int squareRadius)
    {
        var xSet = GenerateDimensionSubset(centre.X, squareRadius);
        var ySet = GenerateDimensionSubset(centre.Y, squareRadius);

        // TODO the map size should be read from config
        var mapSize = 40;
        var positions =
            from x in xSet
            where (0 <= x) && (x < mapSize)
            from y in ySet
            where (0 <= y) && (y < mapSize)
            select new Position() { X = x, Y = y };

        return positions;
    }

    public bool CheckIfPositionIsNextToBotTerritory(Position position, Guid botId)
    {
        var neighbouringPositions = GetNeighbouringPositions(position);
        return neighbouringPositions.Any(neighbour => LandByPosition.ContainsKey(neighbour) && LandByPosition[neighbour].Owner == botId);
    }

    private IEnumerable<Position> GetNeighbouringPositions(Position position)
    {
        return GeneratePositionSquare(position, 1);
    }

    public void AddOccupants(Node node, BotObject bot, int numberOfUnits)
    {
        var territoryId = node.Id;
        var land = GetLandByNodeId(territoryId);
        if (land != node.Position) return;
        
        AddOccupantsToLand(land, bot, numberOfUnits);
    }

    private void AddOccupantsToLand(Land land, BotObject bot, int numberOfUnits)
    {
        if (land.GetOccupantsByBotDictionary().ContainsKey(bot.BotId))
        {
            land.GetOccupantsByBotDictionary()[bot.BotId].Add(numberOfUnits);
        }
        else
        {
            var distanceFromBotBase = CalculationService.CalculateDistanceStatic(land, bot.GetBasePosition());
            var occupants = new Occupants(distanceFromBotBase, bot.BotId)
            {
                Count = numberOfUnits
            };
            occupants.SetOwnsLand(bot.BotId == land.Owner);
            land.GetOccupantsByBotDictionary()[bot.BotId] = occupants;
            land.Occupants.Add(occupants);
        }
    }

    public bool NodeIsInTerritory(Node node)
    {
        return claimedTerritory.Contains(node.Position);
    }

    public void LeaveLand(BotObject bot, Guid territoryId)
    {
        var land = GetLandByNodeId(territoryId);

        var unitsToLeaveLand = land.GetOccupantsByBotDictionary()[bot.BotId].Vacate();
        bot.AvailableUnits += unitsToLeaveLand + 1; // the occupants + the messenger
    }

    public bool HasOccupants(BotObject bot, Land land)
    {
        return land.GetOccupantsByBotDictionary().ContainsKey(bot.BotId) && land.GetOccupantsByBotDictionary()[bot.BotId].Count > 0;
    }
}

public class PositionNotOwnedException : Exception
{
    public Position Position { get; set; }
}
