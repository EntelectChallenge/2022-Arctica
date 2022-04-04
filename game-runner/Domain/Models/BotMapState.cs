using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class BotMapState
    {
        public List<Guid> ScoutTowers { get; set; }
        public List<Guid> Nodes { get; set; }
    }
}