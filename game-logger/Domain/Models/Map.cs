using System.Collections.Generic;

namespace Domain.Models
{
    public class Map
    {
        //Static
        public List<ScoutTower> ScoutTowers { get; set; }

        //NonStatic 
        public List<ResourceNode> Nodes { get; set; }

        public static Map GetStaticFields(Map map)
        {
            return new Map
            {
                ScoutTowers = map.ScoutTowers,
                Nodes = map.Nodes.ConvertAll(node => ResourceNode.GetStaticFields(node))
            };
        }

        public static Map GetVariableFields(Map previousMap, Map currentMap)
        {
            var node = new List<ResourceNode>();

            foreach (var currentNode in currentMap.Nodes)
            {
                var t = ResourceNode.GetVariableFields(previousMap.Nodes.Find(x => x.Id == currentNode.Id), currentNode);

                if (t != null)
                {
                    node.Add(t);
                }
            }


            return new Map
            {
                Nodes = node

            };
        }
    }
}