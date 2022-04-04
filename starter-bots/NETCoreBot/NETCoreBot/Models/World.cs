using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NETCoreBot.Models
{
    public class World
    {
        public int Size { get; set; }
        public int CurrentTick { get; set; }
        public Map Map { get; set; }

        public ScoutTower GetScoutTower(Guid id)
        {
            return Map.ScoutTowers.FirstOrDefault(x => x.Id == id);
        }
    }
}
