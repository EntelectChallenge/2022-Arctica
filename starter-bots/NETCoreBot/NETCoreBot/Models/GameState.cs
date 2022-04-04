using Domain.Models;
using System;
using System.Collections.Generic;

namespace NETCoreBot.Models
{
    public class GameState
    {
        public World World { get; set; }
        public List<BotDto> Bots { get; set; } 
        public Guid BotId { get; set; }
        public List<PopulationTier> PopulationTiers { get; set; }

    }
}