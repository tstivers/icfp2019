using Contest.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Models
{
    public class Problem
    {
        public string ProblemText { get; internal set; }
        public Map Map { get; set; }

        public Robot Robot { get; set; }

        public void ProcessAction(RobotAction action)
        {
            if (action is RobotMoveAction moveAction)
            {
                Robot.Position = Robot.Position.Translate(moveAction.Direction);
                Wrap(Robot.Position);
                WrapArms();
                Actions.Add(action);
            }

            if (action is RobotTurnAction turnAction)
            {
                Robot.Rotate(turnAction.Direction);
                WrapArms();
                Actions.Add(action);
            }
        }

        public void ProcessAction(IEnumerable<RobotAction> actions)
        {
            foreach (var action in actions)
                ProcessAction(action);
        }

        public List<RobotAction> Actions { get; } = new List<RobotAction>();

        public string Solution
        {
            get { return string.Concat(Actions.Select(x => x.ToString())); }
        }

        public string Name { get; set; }
        public List<Point> Targets { get; set; }
        public Point? Target { get; set; }

        public void WrapArms()
        {
            Wrap(Robot.Position.Translate(Robot.Facing));

            if (Robot.Facing == Direction.Right)
            {
                Wrap(Robot.Position.Right().Up());
                Wrap(Robot.Position.Right().Down());
            }

            if (Robot.Facing == Direction.Left)
            {
                Wrap(Robot.Position.Left().Up());
                Wrap(Robot.Position.Left().Down());
            }

            if (Robot.Facing == Direction.Up)
            {
                Wrap(Robot.Position.Up().Left());
                Wrap(Robot.Position.Up().Right());
            }

            if (Robot.Facing == Direction.Down)
            {
                Wrap(Robot.Position.Down().Left());
                Wrap(Robot.Position.Down().Right());
            }
        }

        public void Wrap(Point point)
        {
            if (Map.CellAt(point) == Map.CellType.Empty)
                Map.SetCell(point, Map.CellType.Wrapped);
        }

        public int TryWrap(Point point)
        {
            if (Map.CellAt(point) == Map.CellType.Empty)
                return 1;

            return 0;
        }

        public int ScoreAction(RobotAction action)
        {
            int score = 0;

            if (action is RobotMoveAction moveAction)
            {
                var newPos = Robot.Position.Translate(moveAction.Direction);
                var cellAtNewPos = Map.CellAt(newPos);

                // illegal move
                if (cellAtNewPos == Map.CellType.Wall)
                    return 0;

                // 1 for the robot
                if (cellAtNewPos == Map.CellType.Empty)
                    score = 1;

                // calc arms
                score += TryWrapArms(Robot.Facing, newPos);
            }

            if (action is RobotTurnAction turnAction)
            {
                var newFacing = Robot.Facing.Rotate(turnAction.Direction);
                score += TryWrapArms(newFacing, Robot.Position);
            }

            return score;
        }

        private int TryWrapArms(Direction facing, Point newPos)
        {
            int score = 0;
            if (facing == Direction.Right)
            {
                score += TryWrap(newPos.Right());
                score += TryWrap(newPos.Right().Up());
                score += TryWrap(newPos.Right().Down());
            }

            if (facing == Direction.Left)
            {
                score += TryWrap(newPos.Left());
                score += TryWrap(newPos.Left().Up());
                score += TryWrap(newPos.Left().Down());
            }

            if (facing == Direction.Up)
            {
                score += TryWrap(newPos.Up());
                score += TryWrap(newPos.Up().Left());
                score += TryWrap(newPos.Up().Right());
            }

            if (facing == Direction.Down)
            {
                score += TryWrap(newPos.Down());
                score += TryWrap(newPos.Down().Left());
                score += TryWrap(newPos.Down().Right());
            }

            return score;
        }
    }
}