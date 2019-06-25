using Contest.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.Assert(actions.Count() == 1);

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

        public int ScoreActions(Robot robot, List<RobotAction> actions, HashSet<Point> targets = null)
        {
            var wrapped = new HashSet<Point>();
            var robotPos = robot.Position;
            var robotFacing = robot.Facing;
            var robotArms = robot.Arms;

            foreach (var action in actions)
            {
                if (action is RobotMoveAction moveAction)
                {
                    robotPos = robotPos.Translate(moveAction.Direction);
                    var cellAtNewPos = Map.CellAt(robotPos);

                    // illegal move
                    if (cellAtNewPos == Map.CellType.Wall)
                        return 0;

                    // calc arms
                    TryWrapArms(robotPos, robotArms, wrapped, targets);
                }

                if (action is RobotTurnAction turnAction)
                {
                    robotFacing = robotFacing.Rotate(turnAction.Direction);
                    robotArms = robot.RotateArms(robotArms, turnAction.Direction);

                    TryWrapArms(robotPos, robotArms, wrapped, targets);
                }
            }

            return wrapped.Count;
        }

        private void TryWrapArms(Point pos, List<Point> arms, HashSet<Point> wrapped, HashSet<Point> targets)
        {
            foreach (var arm in arms)
            {
                TryWrap(pos.Translate(arm), wrapped, targets);
            }
        }

        private void TryWrap(Point point, HashSet<Point> wrapped, HashSet<Point> targets)
        {
            if (targets != null)
            {
                if (targets.Contains(point))
                    wrapped.Add(point);
            }
            else
                if (Map.CellAt(point) == Map.CellType.Empty)
                wrapped.Add(point);
        }
    }
}