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
        BotObject CreateBotObject(Guid id);
        BotObject GetBotById(Guid playerActionPlayerId);
        void AddBotObject(BotObject bot);
        GameCompletePayload GenerateGameCompletePayload();
        void Initialize();
        int GetCurrentTick();
        IEnumerable<Guid> GetScoutTowerInformation(Guid id);
        string GetWorldState();
        GameState GetState();
        ResourceNode GetResourceNode(Guid id);
        void AddResourceToMap(ResourceNode resourceNode);
        ScoutTower GetScoutTower(Guid id);
        ResourceNode GetScoutTowerAsResourceNode(Guid id);
        ResourceNode GetBaseAsResourceNode(BotObject bot);
        ResourceNode ResolveNode(PlayerAction playerAction);
    }
}