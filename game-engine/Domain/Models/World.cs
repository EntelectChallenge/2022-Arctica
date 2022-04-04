using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.Models
{
    public class World
    {
        public int Size { get; set; }
        public int CurrentTick { get; set; }
        public Map Map { get; set; }

        public static World Create(IEnumerable<ResourceNode> nodes, IList<ScoutTower> scoutTowers, int size)
        {
            var world = new World
            {
                Size = size,
                Map = new Map
                {
                    Nodes = nodes.ToList(),

                    ScoutTowers = scoutTowers.ToList()
                },
            };

            return world;
        }

        public ScoutTower GetScoutTower(Guid id)
        {
            return Map.ScoutTowers.FirstOrDefault(x => x.Id == id);
        }
    }
}