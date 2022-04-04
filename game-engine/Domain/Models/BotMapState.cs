using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Domain.Models
{
    public class BotMapState
    {
        public List<Guid> ScoutTowers { get; set; }
        public List<Guid> Nodes { get; set; }

        public BotMapState()
        {
            ScoutTowers = new List<Guid>();
            Nodes = new List<Guid>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}