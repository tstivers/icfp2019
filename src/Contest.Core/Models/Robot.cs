using Contest.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Models
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public sealed class Robot
    {
        public Point Position { get; set; }
        public Direction Facing { get; set; }

        public List<Point> Arms { get; set; }

        public Point? Target { get; set; }
        public Point? PriorTarget { get; set; }

        public List<RobotAction> Actions { get; } = new List<RobotAction>();

        public Robot()
        {
            Arms = new List<Point>()
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, -1)
            };
        }

        public void Rotate(Direction direction)
        {
            Facing = Facing.Rotate(direction);
            Arms = RotateArms(direction);
        }

        public List<Point> RotateArms(Direction direction)
        {
            return Arms.Select(x => RotatePoint(x, direction)).ToList();
        }

        private Point RotatePoint(Point point, Direction direction)
        {
            if (direction == Direction.Left)
            {
                return new Point(-point.Y, point.X);
            }
            else if (direction == Direction.Right)
            {
                return new Point(point.Y, -point.X);
            }

            throw new ArgumentException();
        }
    }
}