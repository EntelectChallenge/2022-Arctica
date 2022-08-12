using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Position()
        {
        }
        
        protected bool Equals(Position other)
        {
            return X.Equals(other?.X) && Y.Equals(other?.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }


        public static bool operator == (Position position1, Position position2)
        {
            return !(position1 is null) && position1.Equals(position2);
        }

        public static bool operator != (Position position1, Position position2)
        {
            return !(position1.Equals(position2));
        }
    }

    public class PositionComparer : IEqualityComparer<Position>
    {
        public bool Equals(Position p1, Position p2)
        {
            return !ReferenceEquals(p1, null) && p1.Equals(p2);
        }

        public int GetHashCode(Position obj)
        {
            return obj.GetHashCode();
        }
    }
}