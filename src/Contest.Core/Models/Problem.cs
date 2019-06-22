using System.Collections.Generic;

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
                Map.Cells[Robot.Position.Y][Robot.Position.X] = Map.CellType.Wrapped;
                Robot.Position = Robot.Position.Translate(moveAction.Direction);
            }
        }

        public void ProcessAction(IEnumerable<RobotAction> actions)
        {
            foreach (var action in actions)
                ProcessAction(action);
        }
    }
}