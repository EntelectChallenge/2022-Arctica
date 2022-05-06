using Domain.Models;
using NETCoreBot.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreBot.Models
{
    public class BotDto
    {
        public Guid Id { get; set; }
        public int CurrentTierLevel { get; set; }
        public int Tick { get; set; }
        public BotMapState Map { get; set; }
        public int Population { get; set; }
        public Position BaseLocation { get; set; }
        // public int TravellingUnits { get; set; }
        // public int LumberingUnits { get; set; }
        // public int MiningUnits { get; set; }
        // public int FarmingUnits { get; set; }
        // public int ScoutingUnits { get; set; }
        public List<PlayerActionDto> PendingActions { get; set; }
        public List<PlayerActionDto> Actions { get; set; }
        public int AvailableUnits { get; set; }
        public int Seed { get; set; }
        public int Wood { get; set; }
        public int Food { get; set; }
        public int Stone { get; set; }
        public int Heat { get; set; }
    }
}
