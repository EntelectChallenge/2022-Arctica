using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Domain.Models.DTOs;

namespace Engine.Interfaces
{
    public interface IWorldStateService
    {
        void GenerateMap();
        IList<BotObject> GetPlayerBots();
        void GenerateStartingWorld();
        void ApplyAfterTickStateChanges();
        int GetPlayerCount();
        GameStateDto GetPublishedState();
        BotObject GetBotById(Guid playerActionPlayerId);
        void AddBotObject(BotObject bot);
        GameCompletePayload GenerateGameCompletePayload();
        void Initialize();
        int GetCurrentTick();
        IEnumerable<Guid> GetScoutTowerInformation(Guid id);
        string GetWorldState();
        ISet<Position> GetPositionsInUse();
        void AddPositionInUse(Position position);
        GameState GetState();
        ResourceNode GetResourceNode(Guid id);
        Node GetNode(Guid id);
        void AddResourceToMap(ResourceNode resourceNode);
        ScoutTower GetScoutTower(Guid id);
        ResourceNode GetScoutTowerAsResourceNode(Guid id);
        ResourceNode GetBaseAsResourceNode(BotObject bot);
        Position ResolveNodePosition(PlayerAction playerAction);
        AvailableNode GetAvailableNode(BotObject bot, Guid id);
        void RemoveAvailableNode(Guid availableNodeId);
        void AddAvailableNodes(List<AvailableNode> buildingNodes);
        Position GetNextBotPosition();
        Node NodeByPosition(Position position);
        ScoutTower GetScoutTowerByRegion(Position position);
        ScoutTower CreateScoutTower(int i, int j);
        ISet<Position> ScoutTowerPositionsInUse { get; }
    }
}
