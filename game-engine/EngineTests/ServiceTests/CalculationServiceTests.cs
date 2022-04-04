using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Engine.Handlers.Actions.Retrieval;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
using Engine.Services;
using NUnit.Framework;

namespace EngineTests.ServiceTests
{
    [TestFixture]
    public class CalculationServiceTests : TestBase
    {
        private ActionService actionService;
        private IActionHandlerResolver actionHandlerResolver;
        private List<IActionHandler> actionHandlers;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            actionHandlers = new List<IActionHandler>
            {
                new SendScoutActionHandler(WorldStateService, EngineConfigFake)
            };
            actionHandlerResolver = new ActionHandlerResolver(actionHandlers);
            actionService = new ActionService(WorldStateService, actionHandlerResolver, CalculationService, EngineConfigFake);
        }

        [Test]
        public void GivenZeroUnitsAtResource_ThenReturnZeroDistribution()
        {
            var factor = CalculationService.CalculateDistributionFactor(1, 0);
            
            Assert.AreEqual(0, factor);
        }
        
        [TestCase(1, 1, 1.0)]
        [TestCase(10, 2, 5.0)]
        public void CalculateDistributionFactor_GivenTotalAmountRemaining_AndTotalUnitsAvailable(int calculatedTotalAmount, int totalUnitsAtResource, double expected)
        {
            var actual = CalculationService.CalculateDistributionFactor(calculatedTotalAmount, totalUnitsAtResource);
            
            Assert.AreEqual(expected, actual);
        }
    }
}
