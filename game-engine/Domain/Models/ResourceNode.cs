using System.Collections.Generic;
using System;
using Domain.Enums;

namespace Domain.Models
{
    public class ResourceNode : Node
    {
        public int Amount { get; set; }
        public int Reward { get; set; }
        public int WorkTime { get; set; }

        public RegenerationRate RegenerationRate { get; set; }

        public int CurrentRegenTick { get; set; }
        public int MaxResourceAmount { get; set; }

        public ResourceNode(Position position) : base(GameObjectType.ResourceNode, position)
        {
            CurrentUnits = 0;
        }
    }

    public class RegenerationRate
    {
        public int Ticks { get; set; }
        public int Amount { get; set; }
    }

}