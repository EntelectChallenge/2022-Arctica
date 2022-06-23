using System;
using System.Collections.Generic;

// The player's local map

namespace Domain.Models.DTOs
{
    public class BotDto
    {
        public Guid Id { get; set; }
        public int CurrentTierLevel { get; set; }
        public int Tick { get; set; }
        public BotMapState Map { get; set; }
        public int Population { get; set; }
        public Position BaseLocation { get; set; }
        public List<PlayerActionDto> PendingActions { get; set; }
        public List<PlayerActionDto> Actions { get; set; }
        public List<BuildingObject> Building { get; set; }
        public List<Position> Territory { get; set; }
        public int AvailableUnits { get; set; }
        public int Seed { get; set; }
        public int Wood { get; set; }
        public int Food { get; set; }
        public int Stone { get; set; }
        public int Heat { get; set; }
        public int Gold { get; set; }
    }
}