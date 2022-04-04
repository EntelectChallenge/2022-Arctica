using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Engine.Handlers.Interfaces;

namespace Engine.Handlers.Resolvers
{
    public class ActionHandlerResolver : IActionHandlerResolver
    {
        private readonly IEnumerable<IActionHandler> handlers;

        public ActionHandlerResolver(IEnumerable<IActionHandler> handlers)
        {
            this.handlers = handlers;
        }

        public IActionHandler ResolveHandler(ActionType type)
        {
            var handler = handlers.FirstOrDefault(h => h.IsApplicable(type));

            return handler;
        }
    }
}