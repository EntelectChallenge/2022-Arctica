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
        //TODO: change production speed depending on the number of units sent

        public AvailableNode(Position position) : base(GameObjectType.AvailableNode, position)
        {
           Type = ResourceType.Available;
        }
    }
}
