
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class BuildingObject : GameObject
    {
        public int TerritorySquare { get; set; }
        public BuildingType Type { get; set; }
        public int ScoreMultiplier { get; set; }

        public BuildingObject() { }

        public BuildingObject(
            Position position,
            int territorySquare,
            BuildingType buildingType,
            int scoreMultiplier) : base(Enums.GameObjectType.Building, position)
        {
            TerritorySquare = territorySquare;
            Type = buildingType;
            ScoreMultiplier = scoreMultiplier;

        }
    }
}
