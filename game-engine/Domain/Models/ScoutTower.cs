using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ScoutTower : GameObject
    {
        public IList<Guid> Nodes { get; set; }

        public ScoutTower(): base(GameObjectType.ScoutTower)
        {
            this.Nodes = new List<Guid>();
        }
    }
}