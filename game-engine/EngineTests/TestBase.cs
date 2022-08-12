using System;
using System.Collections.Generic;
using System.IO;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Engine.Handlers.Actions.Retrieval;
using Engine.Handlers.Interfaces;
using Engine.Handlers.Resolvers;
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
        protected TerritoryService TerritoryService;
        protected ObjectGenerationService ObjectGenerationService;
        protected ActionService ActionService;
        public Guid WorldBotId;

        [OneTimeSetUp]
        protected void GlobalPrepare()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testConfig.json", false)
                .Build();
            EngineConfigFake = new ConfigurationService(Options.Create(configuration.Get<EngineConfig>()));
            CalculationService = new CalculationService(EngineConfigFake, TerritoryService);
        }

        protected void Setup()
        {
            var actionHandlers = new List<IActionHandler>
            {
                new SendScoutActionHandler(WorldStateService, EngineConfigFake)
            };
            var actionHandlerResolver = new ActionHandlerResolver(actionHandlers);

            WorldStateService = new WorldStateService(EngineConfigFake);
            CalculationService = new CalculationService(EngineConfigFake, TerritoryService);
            TerritoryService = new TerritoryService(WorldStateService);
            ObjectGenerationService = new ObjectGenerationService(WorldStateService, EngineConfigFake, TerritoryService);
            ActionService = new ActionService(WorldStateService, actionHandlerResolver, CalculationService, EngineConfigFake, TerritoryService);
            FakeGameObjectProvider = new FakeGameObjectProvider(WorldStateService, ObjectGenerationService);

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
                ObjectGenerationService.CreateBotObject(WorldBotId);
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