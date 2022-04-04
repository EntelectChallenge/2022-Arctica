using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ResourceNode : GameObject
    {
        public ResourceType Type { get; set; }

        public int Amount { get; set; }

        public int MaxUnits { get; set; }

        public int CurrentUnits { get; set; }

        public int Reward { get; set; }

        public int WorkTime { get; set; }
        
        public RegenerationRate RegenerationRate { get; set; }
    }
    
    public class RegenerationRate
    {
        public int Ticks { get; set; }
        public int Amount { get; set; }
    }
}