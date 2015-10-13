using System.Drawing;

namespace GreatEscape
{
    public class Wall
    {
        public Point Position { get; set; }

        public Orientation Orientation { get; set; }

        public Wall(Point position, Orientation orientation)
        {
            Position = position;
            Orientation = orientation;
        }
    }
}