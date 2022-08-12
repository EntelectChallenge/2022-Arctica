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
    public class ActionServiceTests : TestBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [Test]
        public void GivenBot_WhenBotHasNoPendingActions_ThenProcessNothing()
        {
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var emptyAction = FakeGameObjectProvider.GetSimpleScoutCommandAction();
            /*bot.AddAction(null);*/


            Assert.DoesNotThrow(() => ActionService.PushPlayerAction(bot.BotId, emptyAction));
        }

        //TODO: Are we using the oldest action or the newest valid action?
        //[Test]
/*        public void GivenBot_WithMultiplePendingActions_ThenOldestActionIsUsed()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetForwardPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction,
                 FakeGameObjectProvider.GetForwardPlayerAction(bot.Id),
                 FakeGameObjectProvider.GetForwardPlayerAction(bot.Id)
             };
            bot.Speed = 1;

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.False(bot.PendingActions.Contains(firstAction));
            Assert.AreEqual(firstAction, bot.CurrentAction);
        }*/

        /*[Test]
        public void GivenBot_WhenIsOnlyBotAlive_ThenProcessNothing()
        {
            SetupFakeWorld(false);
            var bot = FakeGameObjectProvider.GetBotWithActions();

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
        }

        [Test]
        public void GivenBot_WithAfterburnerNotStarted_ThenStartAfterburner()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);
            var botAfter = WorldStateService.GetState().PlayerGameObjects.Find(g => g.Id == bot.Id);

            Assert.IsNotNull(activeEffect);
            Assert.AreEqual(10, activeEffect.Bot.Size);
            Assert.AreEqual(25, activeEffect.Bot.Speed);
            Assert.True(botAfter != default);
            Assert.True(botAfter.Tasks == Task.Afterburner);

            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            Assert.AreEqual(9, activeEffect.Bot.Size);
            Assert.AreEqual(25, activeEffect.Bot.Speed);
            Assert.IsNotNull(botAfter);
            Assert.True(botAfter.Tasks == Task.Afterburner);
        }

        [Test]
        public void GivenBot_WithAfterburnerStarted_ThenStopAfterburner()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            var secondAction = FakeGameObjectProvider.GetStopAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 secondAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);
            var botAfter = WorldStateService.GetState().PlayerGameObjects.Find(g => g.Id == bot.Id);

            Assert.True(activeEffect == default);
            Assert.AreEqual(9, bot.Size);
            Assert.AreEqual(23, bot.Speed);
            Assert.True(botAfter != default);
            Assert.True(botAfter.Tasks != Task.Afterburner);
        }

        [Test]
        public void GivenBot_WithAfterburnerStarted_ThenStartAfterburnerAgain()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            Assert.AreEqual(9, bot.Size);
            Assert.AreEqual(25, bot.Speed);

            var secondAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 secondAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);

            Assert.True(activeEffect != default);
            Assert.AreEqual(9, bot.Size);
            Assert.AreEqual(25, bot.Speed);
        }

        [Test]
        public void GivenBot_WithAfterburnerStarted_ThenProcessTwoTicks()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);

            Assert.True(activeEffect != default);
            Assert.AreEqual(8, bot.Size);
            Assert.AreEqual(28, bot.Speed);
        }

        [Test]
        public void GivenLargeBot_WithAfterburnerStarted_ThenProcessTwoTicks()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            bot.Size = 200;
            bot.Speed = 1;
            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);

            Assert.True(activeEffect != default);
            Assert.AreEqual(198, bot.Size);
            Assert.AreEqual(7, bot.Speed);
        }

        [Test]
        public void GivenBot_WithAfterburnerNotStarted_ThenStopAfterburner()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            var firstAction = FakeGameObjectProvider.GetStopAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);

            Assert.True(activeEffect == default);
            Assert.AreEqual(10, bot.Size);
            Assert.AreEqual(20, bot.Speed);
        }

        [Test]
        public void GivenBot_WithAfterburnerNotStarted_ThenBotIsTooSmall()
        {
            SetupFakeWorld();
            var bot = FakeGameObjectProvider.GetBotAtDefault();
            bot.Size = 5;

            var firstAction = FakeGameObjectProvider.GetStartAfterburnerPlayerAction(bot.Id);
            bot.PendingActions = new List<PlayerAction>
             {
                 firstAction
             };

            Assert.DoesNotThrow(() => actionService.ApplyActionToBot(bot));
            Assert.DoesNotThrow(() => WorldStateService.ApplyAfterTickStateChanges());

            var activeEffect = WorldStateService.GetActiveEffectByType(bot.Id, Task.Afterburner);

            Assert.True(activeEffect == default);
            Assert.AreEqual(5, bot.Size);
            Assert.AreEqual(40, bot.Speed);
        }*/
    }
}