using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ScoutTower : GameObject
    {
        public IList<Guid> Nodes { get; set; }
    }
}