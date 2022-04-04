using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Engine.Handlers.Actions.Retrieval;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
using Engine.Interfaces;
using Engine.Services;
using NUnit.Framework;

namespace EngineTests.HandlerTests
{
    [TestFixture]
    public class ActionHandlerResolverTests : TestBase
    {
        private ActionHandlerResolver actionHandlerResolver;
        private SendScoutActionHandler sendScoutActionHandler;
        private SendLumberActionHandler sendLumberActionHandler;
        private StartCampfireActionHandler startCampfireActionHandler;

        //private NoOpActionHandler noOpActionHandler;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            //noOpActionHandler = new NoOpActionHandler();
            sendScoutActionHandler = new SendScoutActionHandler(WorldStateService, EngineConfigFake);
            sendLumberActionHandler = new SendLumberActionHandler(WorldStateService, EngineConfigFake, CalculationService);
            startCampfireActionHandler = new StartCampfireActionHandler(WorldStateService, EngineConfigFake, CalculationService);
            actionHandlerResolver = new ActionHandlerResolver(
                new List<IActionHandler>
                {
                    sendScoutActionHandler,
                    sendLumberActionHandler,
                    startCampfireActionHandler
                });
        }

        [TestCase(ActionType.Scout, typeof(SendScoutActionHandler))]
        [TestCase(ActionType.Lumber, typeof(SendLumberActionHandler))]
        [TestCase(ActionType.StartCampfire, typeof(StartCampfireActionHandler))]
         public void GivenAction_WhenResolveActionHandler_ThenResolvesSuccessfully(ActionType actionType, Type handlerType)
        {
            var handler = actionHandlerResolver.ResolveHandler(actionType);

            Assert.NotNull(handler);
            Assert.AreEqual(handlerType, handler.GetType());
        }



        //TODO: Should probably implement the noOpActionHandler for edge cases

        /*        [Test]
                public void GivenUnrecognisedAction_WhenResolveActionHandler_ThenResolvesSuccessfully()
                {
                    var action = new PlayerAction
                    {
                        Action = (PlayerActions)9999,
                        Heading = 0,
                        PlayerId = Guid.NewGuid()
                    };

                    var handler = actionHandlerResolver.ResolveHandler(action);

                    Assert.NotNull(handler);
                    Assert.AreEqual(typeof(NoOpActionHandler), handler.GetType());
                }*/
    }
}