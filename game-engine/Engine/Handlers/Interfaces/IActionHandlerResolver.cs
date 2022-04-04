using Domain.Enums;
using Domain.Models;

namespace Engine.Handlers.Interfaces
{
    public interface IActionHandlerResolver
    {
        IActionHandler ResolveHandler(ActionType type);
    }
}