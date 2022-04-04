using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Engine.Handlers.Actions.Retrieval;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
using Engine.Interfaces;
using NUnit.Framework;
using Engine.Services;

namespace EngineTests.HandlerTests
{
    [TestFixture]
    public class SendLumberActionHandlerTests : TestBase
    {
        private SendLumberActionHandler sendLumberActionHandler;
        private ActionHandlerResolver actionHandlerResolver;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            sendLumberActionHandler = new SendLumberActionHandler(WorldStateService, EngineConfigFake, CalculationService);
            
            actionHandlerResolver = new ActionHandlerResolver(
                new List<IActionHandler>
                {
                    sendLumberActionHandler
                });
        }

        [Test]
        public void GivenBot_WithWoodResourceNodeLocation_OnLumberAction_GetWood()
        {           
            /*
             *  -- Generate the world --
            */
            var state = FakeGameObjectProvider.GetFakeWorld();

            /* 
             * -- Get game objects --
             */
            // ScoutTower scoutTower = state.World.ScoutTowers[0];

            var woodNode = FakeGameObjectProvider.GetWoodAt(new Position(0, 0), 1);

            var bot = FakeGameObjectProvider.GetBaseBotAt();

            bot.Map.Nodes.Add(woodNode.Id);
            Assert.AreEqual(1, bot.Map.Nodes.Count);

            /*
            * -- Create and add player action --
            */
            PlayerAction action = FakeGameObjectProvider.GetLumberAction(bot, woodNode.Id, 1, 1);

            // bot.AddAction(action);
     
            sendLumberActionHandler.ProcessActionComplete(woodNode, bot.GetActions());

            /*
            * -- Test --
            */
            Assert.True(bot.Wood > 0);
        }
    }
}
