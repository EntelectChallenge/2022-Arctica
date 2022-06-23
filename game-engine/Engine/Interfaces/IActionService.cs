using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;

namespace Engine.Interfaces
{
    public interface IActionService
    {
        void PushPlayerAction(Guid botId, CommandAction playerAction);
        void HandleCompletedPlayerAction(Node resourceNode, List<PlayerAction> playerAction, ActionType type);
    }
}