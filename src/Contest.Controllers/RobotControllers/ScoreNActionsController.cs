using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class ScoreNActionsController : RobotController, IRobotController
    {
        public List<List<RobotAction>> ActionList = new List<List<RobotAction>>();

        public ScoreNActionsController(Problem problem, IRobotController nextController) : base(problem, nextController)
        {
            var actions = new[]
            {
                RobotAction.Up, RobotAction.Down, RobotAction.Left, RobotAction.Right, RobotAction.TurnLeft,
                RobotAction.TurnRight
            };

            // generate list of all possible 1 move actions
            foreach (var a in actions)
            {
                ActionList.Add(new List<RobotAction> { a });
            }

            // generate all 2 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    ActionList.Add(new List<RobotAction> { a, b });

            // generate all 3 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    foreach (var c in actions)
                        ActionList.Add(new List<RobotAction> { a, b, c });

            // generate all 4 move actions
            foreach (var a in actions)
                foreach (var b in actions)
                    foreach (var c in actions)
                        foreach (var d in actions)
                            ActionList.Add(new List<RobotAction> { a, b, c, d });
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            var bestScore = 0;
            var bestActions = (List<RobotAction>)null;

            foreach (var actions in ActionList)
            {
                var score = Problem.ScoreActions(robot, actions);
                if (score > 1 && score > bestScore)
                {
                    bestScore = score;
                    bestActions = actions;
                }
            }

            if (bestScore > 0)
            {
                return bestActions.Take(1);
            }

            return NextController.GetNextActions(robot).Take(1);
        }
    }
}