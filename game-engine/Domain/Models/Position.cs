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

        public static bool operator ==(Position position1, Position position2)
        {
            if (position1 is null || position2 is null)
                return false;
            return position1.X == position2.X && position1.Y == position2.Y;
        }

        public static bool operator !=(Position position1, Position position2)
        {
            return !(position1 == position2);
        }
    }
}