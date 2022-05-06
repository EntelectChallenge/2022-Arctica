using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models.DTOs;

namespace Domain.Models
{
    public class BotObject : GameObject
    {
        private List<PlayerAction> Actions { get; set; }
        private List<PlayerAction> PendingActions { get; set; }
        public Map Map { get; set; }
        public int AvailableUnits { get; set; }
        public int Score { get; set; }
        public int Placement { get; set; }
        public int Seed { get; set; }

        // Basic resources - can be refactored later
        public int Wood { get; set; }
        public int Food { get; set; }
        public int Gold { get; set; }
        public int Heat { get; set; }
    }
}