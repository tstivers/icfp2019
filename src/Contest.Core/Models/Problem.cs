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
            foreach (var arm in robot.Arms)
                Wrap(robot.Position.Translate(arm));
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
                score += TryWrapArms(newPos, robot.Arms);
            }

            if (action is RobotTurnAction turnAction)
            {
                var newFacing = robot.Facing.Rotate(turnAction.Direction);
                score += TryWrapArms(robot.Position, robot.RotateArms(turnAction.Direction));
            }

            return score;
        }

        private int TryWrapArms(Point newPos, List<Point> arms)
        {
            int score = 0;

            foreach (var arm in arms)
            {
                score += TryWrap(newPos.Translate(arm));
            }

            return score;
        }
    }
}