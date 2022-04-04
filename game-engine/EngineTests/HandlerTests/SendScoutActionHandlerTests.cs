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
    public class SendScoutActionHandlerTests : TestBase
    {
        private SendScoutActionHandler sendScoutActionHandler;
        private ActionHandlerResolver actionHandlerResolver;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            sendScoutActionHandler = new SendScoutActionHandler(WorldStateService, EngineConfigFake);

            actionHandlerResolver = new ActionHandlerResolver(
                new List<IActionHandler>
                {
                    sendScoutActionHandler
                });
        }

        [Test]
        public void GivenBotWithScoutTowerLocations_OnScoutAction_GetResourceNodeLocations()
        {
            /**
             *  -- Generate the world --
            */
            var state = FakeGameObjectProvider.GetFakeWorld();

            /** 
             * -- Get game objects --
             */
            //ScoutTower scoutTower = state.World.ScoutTowers[0];

            //IList<ResourceNode> locations = scoutTower.Nodes;

            var bot = FakeGameObjectProvider.GetBaseBotAt();

            //Assert.AreEqual(1, bot.Map.ScoutTowerLocations.Count);

            /** 
            * -- Create and add player action --
            */
            //PlayerAction action = FakeGameObjectProvider.GetScoutAction(bot, scoutTower.Position, 1, 1);

            // bot.AddAction(action);

            sendScoutActionHandler.ProcessActionComplete(null, bot.GetActions());

            /** 
            * -- Test --
            */
            Assert.True(bot.Map.Nodes.Count >= 3);
        }
    }
}