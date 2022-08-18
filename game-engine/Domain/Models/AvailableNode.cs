using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{
    public class AvailableNode : Node
    {
        private bool isUsed;
        public AvailableNode(Position position) : base(GameObjectType.AvailableNode, position)
        {
           Type = ResourceType.Available;
           isUsed = false;
        }

        public bool IsUsed()
        {
            return isUsed;
        }

        public void Use()
        {
            isUsed = true;
        }
    }
}
