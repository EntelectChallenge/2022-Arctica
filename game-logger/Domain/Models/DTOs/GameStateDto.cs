using System;
using System.Collections.Generic;

namespace Domain.Models.DTOs
{
    public class GameStateDto
    {
        public World World { get; set; }

        public List<BotDto> Bots { get; set; }

        // Used for the bot state
        public Guid BotId { get; set; }

        public List<PopulationTier> PopulationTiers { get; set; }

        public static GameStateDto GetStaticFields(GameStateDto gameStateDto)
        {

            return new GameStateDto
            {
                World = World.GetStaticFields(gameStateDto.World),
                Bots = gameStateDto.Bots.ConvertAll(bot => BotDto.GetStaticFields(bot)),
                BotId = gameStateDto.BotId
            };
        }
        public static GameStateDto GetVariableFields(GameStateDto previousGSDto, GameStateDto currentGSDto)
        {
            var variableWorld = World.GetVariableFields(previousGSDto.World, currentGSDto.World);

            return new GameStateDto
            {
                World = variableWorld,
                Bots = currentGSDto.Bots.ConvertAll(bot => BotDto.GetVariableFields(
                  previousGSDto.Bots.Find(x => x.Id == bot.Id), bot)),
                BotId = currentGSDto.BotId
            };
        }


    }
}