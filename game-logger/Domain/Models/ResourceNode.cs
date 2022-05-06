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

        public static ResourceNode GetStaticFields(ResourceNode rn)
        {
            return new ResourceNode
            {
                Id = rn.Id,
                GameObjectType = rn.GameObjectType,
                Position = rn.Position,
                Type = rn.Type,
                //Amount = rn.Amount, - nonStatic
                MaxUnits = rn.MaxUnits,
                //CurrentUnits = rn.CurrentUnits, - nonstatic 
                Reward = rn.Reward,
                WorkTime = rn.WorkTime,

            };
        }

        public static ResourceNode GetVariableFields(ResourceNode previousRN, ResourceNode newRN)
        {
            if (previousRN.Amount == newRN.Amount || previousRN.CurrentUnits == newRN.CurrentUnits)
            {
                return null;
            }

            return new ResourceNode
            {
                Id = newRN.Id,
                Amount = newRN.Amount,
                CurrentUnits = newRN.CurrentUnits
            };
        }

    }
    public class RegenerationRate
    {
        public int Ticks { get; set; }
        public int Amount { get; set; }
    }
}