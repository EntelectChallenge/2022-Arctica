using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;
using Engine.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace EngineTests.ServiceTests
{
    [TestFixture]
    public class WorldStateServiceTests : TestBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GivenState_WhenGeneratingTheWorldWithASpecificSeed_TheExactSameWorldIsGeneratedTwice(int seed)
        {
            // A bit of a messy way to test seeded randomness, but is there another way??
            // This shows that the service can generate the same list of resources with the same values twice in a row, for different test seeds 
            EngineConfigFake.Value.WorldSeed = seed;

            var dummyWorldStateService = new WorldStateService(EngineConfigFake);

            WorldStateService.GenerateStartingWorld();
            dummyWorldStateService.GenerateStartingWorld();
            
            var firstResourceList = WorldStateService.GetState().World.Map.Nodes;
            var secondResourceList = dummyWorldStateService.GetState().World.Map.Nodes;

            Assert.IsNotEmpty(firstResourceList);
            Assert.IsNotEmpty(secondResourceList);

            // ensure that they are different lists
            Assert.AreNotEqual(firstResourceList, secondResourceList);
            
            // assert that each object is exactly the same and in the same position in the respective array
            for (int i = 0; i < firstResourceList.Count; i++)
            {
                var firstResource = firstResourceList[i];
                var secondResource = secondResourceList[i];
                Assert.IsTrue(firstResource.Position == secondResource.Position);
                Assert.AreEqual(firstResource.Type, secondResource.Type);
                Assert.AreEqual(firstResource.Amount, secondResource.Amount);
            }
        }

        [TestCase(1, 1)]
        [TestCase(2, 4)]
        [TestCase(3, 9)]
        [TestCase(4, 16)]
        public void GivenState_WhenGeneratingScoutTowers_TheNumberOfScoutTowersIsCreatedBasedOnTheConfigValues(int numberOfRegionsInMapLength, int expectedAmountOfScoutTowers)
        {
            EngineConfigFake.Value.NumberOfRegionsInMapLength = numberOfRegionsInMapLength;
            WorldStateService.GenerateMap();

            var towers = WorldStateService.GetState().World.Map.ScoutTowers;
            
            Assert.AreEqual(expectedAmountOfScoutTowers, towers.Count);
        }
        
        // [Test]
        // public void GivenState_WhenGetState_ThenGetsState()
        // {
        //     SetupFakeWorld();
        //     var result = WorldStateService.GetState();
        //     Assert.NotNull(result);
        //     Assert.NotNull(result.World);
        //     Assert.NotNull(result.GameObjects);
        // }

        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnShrunkRadius()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //
        //     var radius = state.World.Radius;
        //
        //     WorldStateService.ApplyAfterTickStateChanges();
        //
        //     Assert.True(state.World.Radius < radius);
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnGrownWormholes()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //
        //     List<Tuple<GameObject, GameObject>> wormholes = state.WormholePairs;
        //     var wormhole = wormholes[0].Item1;
        //     var wormholeSize = wormhole.Size;
        //
        //     WorldStateService.ApplyAfterTickStateChanges();
        //
        //     Assert.True(wormholeSize < state.WormholePairs[0].Item1.Size);
        //     Assert.True(wormholeSize < state.WormholePairs[0].Item2.Size);
        //     Assert.True(state.WormholePairs[0].Item2.Size == state.WormholePairs[0].Item1.Size);
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnWormholesOutOfRadiusRemoved()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //
        //     List<Tuple<GameObject, GameObject>> wormholes = state.WormholePairs;
        //     var wormhole = wormholes[0].Item1;
        //     var distanceFromOrigin = VectorCalculatorService.GetDistanceBetween(new Position(0, 0), wormhole.Position);
        //
        //     for (var i = 0; i < EngineConfigFake.Value.MapRadius - distanceFromOrigin; i++)
        //     {
        //         WorldStateService.ApplyAfterTickStateChanges();
        //     }
        //
        //     Assert.Null(WorldStateService.GetWormholePair(wormhole.Id));
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnFirstWormholePairRemoved()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //
        //     List<Tuple<GameObject, GameObject>> wormholes = state.WormholePairs;
        //     var countBefore = state.WormholePairs.Count;
        //     var distanceFromOrigin = 0;
        //
        //     foreach (Tuple<GameObject, GameObject> t in wormholes)
        //     {
        //         var item1Distance = VectorCalculatorService.GetDistanceBetween(new Position(0, 0), t.Item1.Position) + t.Item1.Size;
        //         var item2Distance = VectorCalculatorService.GetDistanceBetween(new Position(0, 0), t.Item2.Position) + t.Item2.Size;
        //
        //         var longerDistance = item1Distance > item2Distance ? item1Distance : item2Distance;
        //         distanceFromOrigin = distanceFromOrigin > longerDistance ? distanceFromOrigin : longerDistance;
        //     }
        //
        //     var firstSize = wormholes[0].Item1.Size;
        //
        //     for (var i = 0; i < EngineConfigFake.Value.MapRadius - (distanceFromOrigin - firstSize); i++)
        //     {
        //         WorldStateService.ApplyAfterTickStateChanges();
        //     }
        //
        //     Assert.Less(state.WormholePairs.Count, countBefore);
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnFoodRemoved()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //
        //     List<GameObject> objects = state.GameObjects;
        //     var countBefore = objects.Count(a => a.GameObjectType == GameObjectType.Food);
        //     var distanceFromOrigin = 0;
        //
        //     foreach (var food in objects.Where(a => a.GameObjectType == GameObjectType.Food))
        //     {
        //         var itemDistance = VectorCalculatorService.GetDistanceBetween(new Position(0, 0), food.Position);
        //         distanceFromOrigin = distanceFromOrigin > itemDistance ? distanceFromOrigin : itemDistance;
        //     }
        //
        //     for (var i = 0; i < EngineConfigFake.Value.MapRadius - (distanceFromOrigin - EngineConfigFake.Value.WorldFood.FoodSize); i++)
        //     {
        //         WorldStateService.ApplyAfterTickStateChanges();
        //     }
        //
        //     Assert.Less(objects.Count(a => a.GameObjectType == GameObjectType.Food), countBefore);
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenApplyAfterTickStateChanges_ReturnPlayerSmaller()
        // {
        //     WorldStateService.GenerateStartingWorld();
        //     var state = WorldStateService.GetState();
        //     state.World.Radius = 1900;
        //
        //     var bot = FakeGameObjectProvider.GetBotAt(new Position(0, 2000));
        //     bot.Size = 50;
        //     WorldStateService.UpdateBotSpeed(bot);
        //
        //     WorldStateService.ApplyAfterTickStateChanges();
        //     Assert.AreEqual(49, bot.Size);
        //
        //     WorldStateService.ApplyAfterTickStateChanges();
        //     Assert.AreEqual(48, bot.Size);
        //
        //     bot.Position = new Position(0, 1900);
        //     WorldStateService.ApplyAfterTickStateChanges();
        //     Assert.AreEqual(47, bot.Size);
        //
        //     bot.Position = new Position(0, 1849);
        //     WorldStateService.ApplyAfterTickStateChanges();
        //     Assert.AreEqual(47, bot.Size);
        // }
        //
        // [Test]
        // public void GivenWorldState_WhenTickIsTorpedoTick_ThenBotsGetSalvo()
        // {
        //     SetupFakeWorld(true, false);
        //     WorldStateService.GetState().World.CurrentTick = EngineConfigFake.Value.Torpedo.ChargeRate - 1;
        //     WorldStateService.ApplyAfterTickStateChanges();
        //
        //     IList<BotObject> bots = WorldStateService.GetPlayerBots();
        //     Assert.AreEqual(1, bots.First().TorpedoSalvoCount);
        // }
    }
}