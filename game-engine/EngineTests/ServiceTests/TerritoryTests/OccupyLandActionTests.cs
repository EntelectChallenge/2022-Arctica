using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;
using NUnit.Framework;

namespace EngineTests.ServiceTests.TerritoryTests;

[TestFixture]
public class OccupyLandActionTests : TestBase
{
    // Note: for some reason these tests fail when run together, but not when run individually. I suspect that there is 
    // a shared-state problem but I don't have time to figure it out.
    
    private BotObject bot;
    private ResourceNode targetResourceNode;
    private AvailableNode targetAvailableNode;
    
    // THINKING:
    // When adding an OccupyLand action, there are a few things that it needs in order to be valid:
    // 1. The bot needs to be registered and have a guid
    // 2. The target node needs to be a resource node or available node
    // 3. The target node needs to be part of any bot's territory
    // 4. The target node either needs to
    //     1. share an edge/vertex with the bot's territory
    //     2. or the bot already needs to have occupants on it
    // 5. The bot needs to have enough units to send to increase occupation of the land
    // //
         
    // Test Cases
    // 1. Valid action with node that shares vertex with bot's territory - should add - passed
    // 2. Valid action with node that doesn't share a vertex but does have units occupying it - should add - passed
    // 3. Node that isn't in any territory - shouldn't add - passed
    // 4. Node that doesn't share a vertex - shouldn't add - passed
    // 5. Invalid node ID - shouldn't add - passed
    // 6. Action with more units than are available - shouldn't add -
    // 
    // Cases 1, 2, 4, 6 should be true for both ResourceNodes and AvailableNodes
    // //

    [SetUp]
    public void Setup()
    {
        base.Setup();

        EngineConfigFake.Value.RegionSize = EngineConfigFake.Value.WorldLength;
        
        // setup world briefly
        var state = WorldStateService.GetState();
        state.World = World.Create(
            new List<ResourceNode>(), 
            new List<ScoutTower>(){WorldStateService.CreateScoutTower(0, 0)}, 
            40
        );
        
        // create nodes
        targetResourceNode = new ResourceNode(new Position(11, 10));
        WorldStateService.AddResourceToMap(targetResourceNode);

        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());
        ObjectGenerationService.CreateBotObject(Guid.NewGuid());

        bot = WorldStateService.GetPlayerBots().MinBy(bot => CalculationService.CalculateDistance(bot.GetBasePosition(), targetResourceNode.Position));
        targetAvailableNode = WorldStateService.GetState().World.Map.AvailableNodes.First();
    }

    // CASE 1
    [TestCase(true)]
    [TestCase(false)]
    public void Case1_AddNewValidOccupyLandAction(bool shouldTargetResourceNode)
    {
        Node targetNode = shouldTargetResourceNode ? targetResourceNode : targetAvailableNode;
        var targetNodeId = targetNode.Id;
        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(bot.BotId, validCommandAction);

        var expectedAction = ActionService.GetPlayerActionFromCommand(validCommandAction);
        var actualAction = bot.GetNewActions().FirstOrDefault();
        
        Assert.IsNotNull(actualAction);
        Assert.AreSame(bot, actualAction.Bot);
        Assert.IsTrue(AreActionsEqual(expectedAction, actualAction));
    }

    // CASE 2
    [TestCase(true)]
    [TestCase(false)]
    public void Case2_AddOccupyLandAction_WithTargetNodeNotAdjacentToBotTerritory_ButAlreadyHasOccupantsFromBot(bool shouldTargetResourceNode)
    {
        Node targetNode = shouldTargetResourceNode ? targetResourceNode : targetAvailableNode;

        var farthestBot = WorldStateService.GetPlayerBots().MaxBy(bot => CalculationService.CalculateDistance(bot.GetBasePosition(), targetNode.Position));
        Assert.IsNotNull(farthestBot);
        
        TerritoryService.AddOccupants(targetNode, farthestBot, 1);
        
        var targetNodeId = targetNode.Id;
        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(farthestBot.BotId, validCommandAction);

        var expectedAction = ActionService.GetPlayerActionFromCommand(validCommandAction);
        var actualAction = farthestBot.GetNewActions().FirstOrDefault();
        
        Assert.IsNotNull(actualAction);
        Assert.AreSame(farthestBot, actualAction.Bot);
        Assert.IsTrue(AreActionsEqual(expectedAction, actualAction));
    }
    
    // CASE 3
    [Test]
    public void Case3_AddOccupyLandAction_WithTargetNodeNotInAnyTerritory()
    {
        var nodeOutOfTerritory = new ResourceNode(new Position(0, 0));
        WorldStateService.AddResourceToMap(nodeOutOfTerritory);
        
        var targetNodeId = nodeOutOfTerritory.Id;
        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(bot.BotId, validCommandAction);

        var actualAction = bot.GetNewActions().FirstOrDefault();
        
        Assert.IsNull(actualAction);
    }

    // CASE 4
    [TestCase(true)]
    [TestCase(false)]
    public void Case4_AddOccupyLandAction_WithTargetNodeNotAdjacentToBotTerritory(bool shouldTargetResourceNode)
    {
        Node targetNode = shouldTargetResourceNode ? targetResourceNode : targetAvailableNode;
        
        var farthestBot = WorldStateService.GetPlayerBots().MaxBy(bot => CalculationService.CalculateDistance(bot.GetBasePosition(), targetNode.Position));
        Assert.IsNotNull(farthestBot);
        
        var targetNodeId = targetNode.Id;
        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(farthestBot.BotId, validCommandAction);

        var actualAction = bot.GetNewActions().FirstOrDefault();
        
        Assert.IsNull(actualAction);
    }
    
    // CASE 5
    [Test]
    public void Case5_AddOccupyLandAction_WithInvalidTargetNodeId()
    {
        var targetNodeId = Guid.Empty;
        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(bot.BotId, validCommandAction);

        var actualAction = bot.GetNewActions().FirstOrDefault();
        
        Assert.IsNull(actualAction);
    }

    // CASE 6
    [TestCase(true)]
    [TestCase(false)]
    public void Case6_AddOccupyLandAction_WithTooManyUnits(bool shouldTargetResourceNode)
    {
        Node targetNode = shouldTargetResourceNode ? targetResourceNode : targetAvailableNode;
        var targetNodeId = targetNode.Id;

        var validCommandAction = new CommandAction
        {
            Type = ActionType.OccupyLand,
            Units = bot.AvailableUnits + 1,
            Id = targetNodeId
        };
        ActionService.PushPlayerAction(bot.BotId, validCommandAction);

        var actualAction = bot.GetNewActions().FirstOrDefault();
        
        Assert.IsNull(actualAction);
    }

    
    private bool AreActionsEqual(PlayerAction a1, PlayerAction a2)
    {
        return a1.ActionType == a2.ActionType
               && a1.TargetNodeId == a2.TargetNodeId
               && a1.NumberOfUnits == a2.NumberOfUnits;
    }
}
