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

        public static RobotAction TurnLeft => new RobotTurnAction(Direction.Left);
        public static RobotAction TurnRight => new RobotTurnAction(Direction.Right);
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

    public class RobotTurnAction : RobotAction
    {
        public Direction Direction { get; }

        public RobotTurnAction(Direction direction)
        {
            Direction = direction;
        }

        public override string ToString()
        {
            switch (Direction)
            {
                case Direction.Left: return "Q";
                case Direction.Right: return "E";
            }
            throw new ArgumentException();
        }
    }

    public class RobotDoneAction : RobotAction
    {
    }
}