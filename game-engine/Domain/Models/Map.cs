using System;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Map
    {
        public List<ScoutTower> ScoutTowers { get; set; }
        public List<ResourceNode> Nodes { get; set; }
        public List<AvailableNode> AvailableNodes { get; set; }

        public Map()
        {
            ScoutTowers = new List<ScoutTower>();
            Nodes = new List<ResourceNode>();
            AvailableNodes = new List<AvailableNode>();
        }


        public bool Exists(Guid id)
        {
            return AvailableNodes.Exists(x => x.Id == id);
        }

        public AvailableNode ResolveAvailableNode(Guid id)
        {
            return AvailableNodes.FirstOrDefault(x => x.Id == id);
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public ScoutTower GetScoutTowerByPosition(Position position)
        {
            return ScoutTowers.FirstOrDefault(x => x.Position == position);
        }

        public void AddNodes(IEnumerable<ResourceNode> newNodes)
        {
            this.Nodes.AddRange(newNodes);
        }

        public ResourceNode ResolveResourceNode(Guid id)
        {
            return Nodes.FirstOrDefault(node => node.Id == id);
        }

        public Node ResolveNode(Guid id)
        {
            var resourceNode = Nodes.FirstOrDefault(node => node.Id == id);
            var availableNode = AvailableNodes.FirstOrDefault(node => node.Id == id);

            return resourceNode == null ? availableNode : resourceNode;
        }

        public ResourceNode ResolveBuildingNode(Guid id)
        {
            return Nodes.FirstOrDefault(node => node.Id == id);
        }
    }
}