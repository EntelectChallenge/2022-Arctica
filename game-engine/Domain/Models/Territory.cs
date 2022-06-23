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

    public void AddBuilding(BuildingObject building)
    {
        var buildingTerritory = building.GetPositionsInBuildingRadius();
        PositionsInTerritory.UnionWith(buildingTerritory);
    }
    
    public bool Contains(Position position)
    {
        return PositionsInTerritory.Contains(position);
    }



}
