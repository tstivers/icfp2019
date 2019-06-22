using Contest.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Models
{
    public class Problem
    {
        public string ProblemText { get; internal set; }
        public Map Map { get; set; }
        public List<Robot> Robots { get; set; } = new List<Robot>();
        public string Name { get; set; }

        public void ProcessAction(Robot robot, RobotAction action)
        {
            if (action is RobotMoveAction moveAction)
            {
                robot.Position = robot.Position.Translate(moveAction.Direction);
                Wrap(robot.Position);
                WrapArms(robot);
                robot.Actions.Add(action);
            }

            if (action is RobotTurnAction turnAction)
            {
                robot.Rotate(turnAction.Direction);
                WrapArms(robot);
                robot.Actions.Add(action);
            }
        }

        public void ProcessAction(Robot robot, IEnumerable<RobotAction> actions)
        {
            foreach (var action in actions)
                ProcessAction(robot, action);
        }

        public string Solution
        {
            get
            {
                return string.Join("#", Robots.Select(x => string.Concat(x.Actions.Select(y => y.ToString()))));
            }
        }

        public void WrapArms(Robot robot)
        {
            Wrap(robot.Position.Translate(robot.Facing));

            if (robot.Facing == Direction.Right)
            {
                Wrap(robot.Position.Right().Up());
                Wrap(robot.Position.Right().Down());
            }

            if (robot.Facing == Direction.Left)
            {
                Wrap(robot.Position.Left().Up());
                Wrap(robot.Position.Left().Down());
            }

            if (robot.Facing == Direction.Up)
            {
                Wrap(robot.Position.Up().Left());
                Wrap(robot.Position.Up().Right());
            }

            if (robot.Facing == Direction.Down)
            {
                Wrap(robot.Position.Down().Left());
                Wrap(robot.Position.Down().Right());
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

        public int ScoreAction(Robot robot, RobotAction action)
        {
            int score = 0;

            if (action is RobotMoveAction moveAction)
            {
                var newPos = robot.Position.Translate(moveAction.Direction);
                var cellAtNewPos = Map.CellAt(newPos);

                // illegal move
                if (cellAtNewPos == Map.CellType.Wall)
                    return 0;

                // 1 for the robot
                if (cellAtNewPos == Map.CellType.Empty)
                    score = 1;

                // calc arms
                score += TryWrapArms(robot.Facing, newPos);
            }

            if (action is RobotTurnAction turnAction)
            {
                var newFacing = robot.Facing.Rotate(turnAction.Direction);
                score += TryWrapArms(newFacing, robot.Position);
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