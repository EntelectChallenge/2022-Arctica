using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Models.DTOs
{
    public class GameStateDto
    {
        public World World { get; set; }
        public List<BotDto> Bots { get; set; }
        public Guid BotId { get; set; }
        public List<PopulationTier> PopulationTiers { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}