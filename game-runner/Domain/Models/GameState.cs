using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class GameState
    {
        public World World { get; set; }
        public List<BotObject> Bots { get; set; } // [ size, x, y, speed, type, id] DTO
    }
}