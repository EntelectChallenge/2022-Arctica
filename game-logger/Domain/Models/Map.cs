using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Map
    {
        public List<ScoutTower> ScoutTowers { get; set; }
        public List<ResourceNode> Nodes { get; set; }
    }
}