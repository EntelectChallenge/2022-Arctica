using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NETCoreBot.Models
{
    public class Map
    {
        public List<ScoutTower> ScoutTowers { get; set; }
        public List<ResourceNode> Nodes { get; set; }

        public Map()
        {
            ScoutTowers = new List<ScoutTower>();
            Nodes = new List<ResourceNode>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}