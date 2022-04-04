using NETCoreBot.Enums;
using NETCoreBot.Models;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ScoutTower : GameObject
    {
        public IList<Guid> Nodes { get; set; }
        public ScoutTower() : base(GameObjectType.ScoutTower)
        {
            this.Nodes = new List<Guid>();
        }
    }
}