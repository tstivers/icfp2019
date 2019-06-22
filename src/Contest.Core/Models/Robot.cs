using Contest.Core.Extensions;
using System.Collections.Generic;

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

        public Point? Target { get; set; }
        public Point? PriorTarget { get; set; }

        public List<RobotAction> Actions { get; } = new List<RobotAction>();

        public void Rotate(Direction direction)
        {
            Facing = Facing.Rotate(direction);
        }
    }
}