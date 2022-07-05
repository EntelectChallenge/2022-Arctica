using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models;

public class Territory
{
    public readonly HashSet<Position> PositionsInTerritory = new(new PositionComparer());


    public bool AddPosition(Position position)
    {
        return PositionsInTerritory.Add(position);
    }

    public void AddBuilding(BuildingObject building, ISet<Position> claimedPositions)
    {
        var buildingTerritory = building.GetPositionsInBuildingRadius();
        PositionsInTerritory.UnionWith(buildingTerritory.Except(claimedPositions));
    }
    
    public bool Contains(Position position)
    {
        return PositionsInTerritory.Contains(position);
    }



}
