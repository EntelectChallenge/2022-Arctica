using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Domain.Models;

public class Territory
{
    public readonly HashSet<Land> LandInTerritory = new(new PositionComparer());


    public bool AddLand(Land land)
    {
        return LandInTerritory.Add(land);
    }

    public bool Contains(Position position)
    {
        return LandInTerritory.Contains(position);
    }

    public bool RemoveLand(Land land)
    {
        return LandInTerritory.Remove(land);
    }
}
