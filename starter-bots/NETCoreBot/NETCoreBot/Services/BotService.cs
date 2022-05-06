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
        private EngineConfigDto _engineConfigDto;

        public BotService()
        {
            _playerAction = new CommandAction();
            _playerCommand = new PlayerCommand();
            _gameState = new GameState();
            _bot = new BotDto();
            _engineConfigDto = new EngineConfigDto();
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
        
        public EngineConfigDto GetEngineConfigDto()
        {
            return _engineConfigDto;
        }

        public void SetEngineConfigDto(EngineConfigDto engineConfigDto)
        {
            _engineConfigDto = engineConfigDto;
        }

    }
}