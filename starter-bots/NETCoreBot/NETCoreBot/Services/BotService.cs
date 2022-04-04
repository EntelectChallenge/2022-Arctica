using Domain.Models;
using NETCoreBot.Enums;
using NETCoreBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NETCoreBot.Services
{
    public class BotService
    {
        private BotDto _bot;
        private CommandAction _playerAction;
        private PlayerCommand _playerCommand;
        private GameState _gameState;

        public BotService()
        {
            _playerAction = new CommandAction();
            _playerCommand = new PlayerCommand();
            _gameState = new GameState();
            _bot = new BotDto();
        }

        public BotDto GetBot()
        {
            return _bot;
        }

        public PlayerCommand GetPlayerCommand()
        {

            PlayerCommand playerCommand = new PlayerCommand();

            if (this._gameState.World != null)
            {
                if (this._gameState.World.Map.Nodes.Count > 0)
                {
                    playerCommand.Actions.Add(new CommandAction()
                    {
                        Type = ActionType.Farm,
                        Units = this._gameState.Bots.FirstOrDefault(go => go.Id == _bot.Id).AvailableUnits,
                        Id = this._gameState.World.Map.Nodes[0].Id,
                    });

                    playerCommand.PlayerId = this._bot.Id;
                }
                else
                {
                    playerCommand.Actions.Add(new CommandAction()
                    {
                        Type = ActionType.Scout,
                        Units = 1,
                        Id = this._gameState.World.Map.ScoutTowers[0].Id,
                    });

                    playerCommand.PlayerId = this._bot.Id;
                }
            }

            return playerCommand;
        }

        public void SetBot(BotDto bot)
        {
            _bot.Id = bot.Id;
        }

        public void ComputeNextPlayerAction(PlayerCommand playerCommand)
        {

            _playerAction = playerCommand.Actions[0];
        }

        public GameState GetGameState()
        {
            return _gameState;
        }

        public void SetGameState(GameState gameState)
        {
            _gameState = gameState;
            UpdateSelfState();
        }

        private void UpdateSelfState()
        {
            _bot = _gameState.Bots.FirstOrDefault(go => go.Id == _bot.Id);
        }

        private double GetDistanceBetween(GameObject location1, GameObject location2)
        {
            Position baseLocation = location1.Position;
            Position nodeLocation = location2.Position;

            double deltaX = baseLocation.X - nodeLocation.X;
            double deltaY = baseLocation.Y - nodeLocation.Y;
            var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);

            double distance = Math.Sqrt(distanceSquared);

            return distance;
        }

        private double GetDistanceBetweenBaseAndNode(Node node)
        {

            Position baseLocation = this._bot.BaseLocation;
            Position nodeLocation = node.Position;

            double deltaX = baseLocation.X - nodeLocation.X;
            double deltaY = baseLocation.Y - nodeLocation.Y;
            var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);

            double distance = Math.Sqrt(distanceSquared);

            return distance;
        }

    }
}