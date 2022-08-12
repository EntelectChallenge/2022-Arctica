using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using NUnit.Framework;

namespace EngineTests.ServiceTests.TerritoryTests;

[TestFixture]
public class TerritoryRecalculationTests : TestBase
{
    [SetUp]
    public void Setup()
    {
        base.Setup();
    }
    
    [Test]
    public void TerritoryRecalculationTest() // Note: if there is a change in the Occupants.Pressure formula or its weightings then this test might start failing
    {
        EngineConfigFake.Value.RegionSize = EngineConfigFake.Value.WorldLength;
        
        // setup world briefly
        var state = WorldStateService.GetState();
        state.World = World.Create(
            new List<ResourceNode>(), 
            new List<ScoutTower>(){WorldStateService.CreateScoutTower(0, 0)}, 
            40
            );

        
        // create nodes
        var node1 = new ResourceNode(new Position(11, 10));
        WorldStateService.AddResourceToMap(node1);
        
        // setup the bots
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());

        var closestBot = WorldStateService.GetPlayerBots().MinBy(bot => CalculationService.CalculateDistance(bot.GetBasePosition(), node1.Position));

        var bot = closestBot;
        var competingBot = WorldStateService.GetPlayerBots().First(bot => bot != closestBot);

        var botTerritory = bot.Territory.LandInTerritory.ToList();
        Assert.IsNotEmpty(botTerritory);
        
        
        
        // CASE 1: Test with ResourceNode
        Assert.IsTrue(TerritoryService.NodeIsInTerritory(node1));
        
        // check that the contested land is correctly owned
        var contestedNode = node1;
        var contestedLand = TerritoryService.GetLandByNodeId(node1.Id);
        
        Assert.AreEqual(bot.BotId, contestedLand.Owner);
        Assert.AreEqual(0, contestedLand.GetOccupantsByBotDictionary()[bot.BotId].Count);
        
        // add competing occupants and check that the land's occupants have increased for the competing bot
        TerritoryService.AddOccupants(contestedNode, competingBot, 11);
        Assert.AreEqual(11, contestedLand.GetOccupantsByBotDictionary()[competingBot.BotId].Count);

        TerritoryService.RecalculateTerritories();
        Assert.AreEqual(competingBot.BotId, contestedLand.Owner);

        // Original owner takes back the territory
        TerritoryService.AddOccupants(contestedNode, bot, 1);
        Assert.AreEqual(1, contestedLand.GetOccupantsByBotDictionary()[bot.BotId].Count);
        
        TerritoryService.RecalculateTerritories();
        Assert.AreEqual(bot.BotId, contestedLand.Owner);
        
        // Original bot leaves land and competing bot gets it
        
        TerritoryService.LeaveLand(bot, contestedNode.Id);
        Assert.AreEqual(0, contestedLand.GetOccupantsByBotDictionary()[bot.BotId].Count);
        
        TerritoryService.RecalculateTerritories();
        Assert.AreEqual(competingBot.BotId, contestedLand.Owner);
        
        
        
        
        // CASE 2: Test with AvailableNode

        // check that the contested land is correctly owned
        var contestedAvailableNode = WorldStateService.GetNode(bot.Map.AvailableNodes[0]);
        Assert.IsTrue(TerritoryService.NodeIsInTerritory(contestedAvailableNode));
        contestedLand = TerritoryService.GetLandByNodeId(contestedAvailableNode.Id);
        
        Assert.AreEqual(bot.BotId, contestedLand.Owner);
        Assert.AreEqual(0, contestedLand.GetOccupantsByBotDictionary()[bot.BotId].Count);
        
        // add competing occupants and check that the land's occupants have increased for the competing bot
        TerritoryService.AddOccupants(contestedAvailableNode, competingBot, 11);
        Assert.AreEqual(11, contestedLand.GetOccupantsByBotDictionary()[competingBot.BotId].Count);

        TerritoryService.RecalculateTerritories();
        Assert.AreEqual(competingBot.BotId, contestedLand.Owner);

        // Original owner takes back the territory
        TerritoryService.AddOccupants(contestedAvailableNode, bot, 10);
        Assert.AreEqual(10, contestedLand.GetOccupantsByBotDictionary()[bot.BotId].Count);
        
        TerritoryService.RecalculateTerritories();
        Assert.AreEqual(bot.BotId, contestedLand.Owner);
    }
}