
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

        private IEnumerable<int> GenerateDimensionSubset(int centre, int size)
        {
            var start = centre - size;
            var count = size * 2 + 1;
            return Enumerable.Range(start, count);
        }

        public IEnumerable<Position> GetPositionsInBuildingRadius()
        {
            var xSet = GenerateDimensionSubset(Position.X, TerritorySquare);
            var ySet = GenerateDimensionSubset(Position.Y, TerritorySquare);

            // TODO the max region size should be read from config
            var positions =
                from x in xSet
                where (0 <= x) && (x < 40)
                from y in ySet
                where (0 <= y) && (y < 40)
                select new Position() { X = x, Y = y };

            return positions;
        }


    }
}
