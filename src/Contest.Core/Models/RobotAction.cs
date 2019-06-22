using System;

namespace Contest.Core.Models
{
    public abstract class RobotAction
    {
        public static RobotAction Up => new RobotMoveAction(Direction.Up);
        public static RobotAction Down => new RobotMoveAction(Direction.Down);
        public static RobotAction Left => new RobotMoveAction(Direction.Left);
        public static RobotAction Right => new RobotMoveAction(Direction.Right);
        public static RobotAction Done => new RobotDoneAction();
    }

    public class RobotMoveAction : RobotAction
    {
        public Direction Direction { get; }

        public RobotMoveAction(Direction direction)
        {
            Direction = direction;
        }

        public override string ToString()
        {
            switch (Direction)
            {
                case Direction.Down: return "S";
                case Direction.Up: return "W";
                case Direction.Left: return "A";
                case Direction.Right: return "D";
            }
            throw new ArgumentException();
        }
    }

    public class RobotDoneAction : RobotAction
    {
    }
}