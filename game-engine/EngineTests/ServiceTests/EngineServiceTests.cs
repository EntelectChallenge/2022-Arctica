using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
using Engine.Services;
using NUnit.Framework;

namespace EngineTests.ServiceTests
{
    [TestFixture]
    public class EngineServiceTests : TestBase
    {
        private ResourceNode node;
        private EngineService engineService;
        [SetUp]
        public void Setup()
        {
            base.Setup();
            SetupFakeWorld();
            node = new ResourceNode(new Position(0, 0))
            {
                MaxUnits = 10,
                Type = ResourceType.Stone
            };
            
            engineService = new EngineService(
                WorldStateService, 
                ActionService, 
                EngineConfigFake,
                new TickProcessingService(WorldStateService), 
                CalculationService, 
                TerritoryService
                );
            
            
        }
        
        // [TestCaseSource(nameof(GetPlayerActionTestCases))]
        [TestCase(1000, new []{800, 700}, new []{533, 466})]
        [TestCase(1, new []{1}, new []{1})]
        [TestCase(2, new []{1, 1}, new []{1, 1})]
        [TestCase(0, new []{1, 1}, new []{0, 0})]
        [TestCase(2, new []{4, 4}, new []{1, 1})]
        [TestCase(100, new []{4, 4}, new []{4, 4})]
        [TestCase(100, new []{100, 10}, new []{90, 9})]
        [TestCase(3, new []{2, 2}, new []{1, 1})]
        public void SlotDistributionTest(int availableSpace, int[] numberOfUnitsList, int[] distributedUnitsList)
        {
            Assert.AreEqual(numberOfUnitsList.Length, distributedUnitsList.Length);

            node.MaxUnits = availableSpace;
            node.CurrentUnits = 0;
            
            WorldStateService.AddResourceToMap(node);
            var actions = numberOfUnitsList.Select(numberOfUnits => new PlayerAction(ActionType.Mine, numberOfUnits, node.Id){Bot = new BotObject()}).ToList();
            engineService.DistributeNodeSpaceAmongActions(node, actions);

            for (int i = 0; i < actions.Count; i++)
            {
                Assert.AreEqual(actions[i].NumberOfUnits, distributedUnitsList[i]);
            }
        }
    }
}