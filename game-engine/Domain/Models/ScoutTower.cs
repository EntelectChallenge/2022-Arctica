using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ScoutTower : GameObject
    {
        public IList<Guid> Nodes { get; set; }
        
        public Dictionary<Guid, Territory> TerritoryNodes { get; set; }
        public int XRegion {get; set;}
        public int YRegion {get; set;}  

        public ScoutTower(Position position) : base(GameObjectType.ScoutTower, position)
        {
            this.Nodes = new List<Guid>();
            this.TerritoryNodes = new Dictionary<Guid, Territory>();
        }

        public void AddTerritoryNode(Guid botId, Position position)
        {
            if(TerritoryNodes[botId] == null)
            {
                TerritoryNodes[botId] = new Territory();
            }

            TerritoryNodes[botId].AddPosition(position);
        }
    }
}