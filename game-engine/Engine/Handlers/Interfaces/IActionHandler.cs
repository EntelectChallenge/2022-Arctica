using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;

namespace Engine.Handlers.Interfaces
{
    public interface IActionHandler
    {
        bool IsApplicable(ActionType type);
        void ProcessActionComplete(ResourceNode resourceNode, List<PlayerAction> playerAction);
    }
}