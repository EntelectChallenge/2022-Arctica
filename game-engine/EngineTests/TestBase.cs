using System;
using System.Collections.Generic;
using System.IO;
using Domain.Enums;
using Domain.Models;
using Engine.Interfaces;
using Engine.Models;
using Engine.Services;
using EngineTests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace EngineTests
{
    public class TestBase
    {
        protected IWorldStateService WorldStateService;
        protected IConfigurationService EngineConfigFake;
        protected FakeGameObjectProvider FakeGameObjectProvider;
        protected ICalculationService CalculationService;
        public Guid WorldBotId;

        [OneTimeSetUp]
        protected void GlobalPrepare()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testConfig.json", false)
                .Build();
            EngineConfigFake = new ConfigurationService(Options.Create(configuration.Get<EngineConfig>()));
            CalculationService = new CalculationService(EngineConfigFake);
        }

        protected void Setup()
        {
            //VectorCalculatorService = new VectorCalculatorService();
            WorldStateService = new WorldStateService(EngineConfigFake);
            FakeGameObjectProvider = new FakeGameObjectProvider(WorldStateService);
            CalculationService = new CalculationService(EngineConfigFake);
        }

        protected void SetupFakeWorld(bool withABot = true, bool withFood = true)
        {
            if (withFood)
            {
                WorldStateService.GenerateStartingWorld();
            }

            if (withABot)
            {
                WorldBotId = Guid.NewGuid();
                WorldStateService.CreateBotObject(WorldBotId);
            }

           // WorldStateService.GetPublishedState.WormholePairs = new List<Tuple<GameObject, GameObject>>();
        }

/*        protected GameObject PlaceFoodAtPosition(Position position)
        {
            var gameObject = new GameObject
            {
                Id = Guid.NewGuid(),
                Position = position,
                GameObjectType = GameObjectType.Food,
                Size = EngineConfigFake.Value.WorldFood.FoodSize
            };
            WorldStateService.AddGameObject(gameObject);
            return gameObject;
        }*/
    }
}