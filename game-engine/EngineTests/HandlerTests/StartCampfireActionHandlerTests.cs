using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
using Engine.Interfaces;
using NUnit.Framework;
using Engine.Services;

namespace EngineTests.HandlerTests
{
    [TestFixture]
    public class StartCampfireActionHandlerTests : TestBase
    {
        private StartCampfireActionHandler startCampfireActionHandler;
        private ActionHandlerResolver actionHandlerResolver;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            startCampfireActionHandler = new StartCampfireActionHandler(WorldStateService, EngineConfigFake, CalculationService);
            
            actionHandlerResolver = new ActionHandlerResolver(
                new List<IActionHandler>
                {
                    startCampfireActionHandler
                });
        }

        [Test]
        public void GivenBot_WithZeroHeat_OnStartCampfireAction_IncreaseBaseHeat()
        {           
            /*
             *  -- Generate the world --
            */
            var state = FakeGameObjectProvider.GetFakeWorld();

            /* 
             * -- Get game objects --
             */
            // ScoutTower scoutTower = state.World.ScoutTowers[0];
            
            var bot = FakeGameObjectProvider.GetBaseBotAt();

            /*
            * -- Create and add player action --
            */
            var numberOfUnits = 2;
            PlayerAction action = FakeGameObjectProvider.GetStartCampfireAction(bot, Guid.Empty, numberOfUnits, 1);

            ActionService.AddAction(bot, action, null);
            
            bot.Wood = numberOfUnits * EngineConfigFake.Value.ResourceGenerationConfig.Campfire.RewardRange[0];
     
            startCampfireActionHandler.ProcessActionComplete(null, bot.GetActions());

            /*
            * -- Test --
            */
            Assert.Zero(bot.Wood);
            Assert.True(bot.Heat > 0);
        }
    }
}
