using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ScoutTower : GameObject
    {
        public IList<Guid> Nodes { get; set; }
        public IList<Land> Territory { get; set; }
        public int XRegion { get; set; }
        public int YRegion { get; set; }

        public ScoutTower(Position position) : base(GameObjectType.ScoutTower, position)
        {
            this.Nodes = new List<Guid>();
            this.Territory = new List<Land>();
        }

        public void AddTerritoryNode(Land land)
        {
            Territory.Add(land);
        }
    }
}