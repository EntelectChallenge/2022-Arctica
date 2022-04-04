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
    }
}