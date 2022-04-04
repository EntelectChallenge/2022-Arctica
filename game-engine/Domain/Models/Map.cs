using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Domain.Models
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

        public void AddNodes(IEnumerable<ResourceNode> newNodes)
        {
            this.Nodes.AddRange(newNodes);
        }

        public ResourceNode ResolveNode(Guid id)
        {
            return Nodes.FirstOrDefault(node => node.Id == id);
        }
    }
}