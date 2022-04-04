using System.Collections.Generic;
using Domain.Models.DTOs;

namespace Domain.Models
{
    public class GameCompletePayload
    {
        public int TotalTicks { get; set; }
        public List<PlayerResult> Players { get; set; }
        public List<int> WorldSeeds { get; set; }
        public BotDto WinningBot { get; set; }
    }
}