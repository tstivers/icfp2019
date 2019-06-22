using Contest.Core.Extensions;

namespace Contest.Core.Models
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class Robot
    {
        public Point Position { get; set; }

        public Direction Facing { get; set; }

        public Point Front => Position.Translate(Facing);

        public void Rotate(Direction direction)
        {
            Facing = Facing.Rotate(direction);
        }
    }
}