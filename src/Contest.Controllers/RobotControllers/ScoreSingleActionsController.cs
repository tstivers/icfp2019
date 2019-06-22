using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class ScoreSingleActionsController : IRobotController
    {
        public Problem Problem { get; }
        public IRobotController NextController { get; }

        public ScoreSingleActionsController(Problem problem, IRobotController nextController)
        {
            Problem = problem;
            NextController = nextController;
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            // get scores for all actions
            var actions = new[]
            {
                RobotAction.Up, RobotAction.Down, RobotAction.Left, RobotAction.Right, RobotAction.TurnLeft,
                RobotAction.TurnRight
            };

            var bestScore = 0;
            var bestAction = (RobotAction)null;

            for (int i = 0; i < actions.Length; i++)
            {
                var score = Problem.ScoreAction(robot, actions[i]);
                if (score > 1 && score >= bestScore)
                {
                    bestScore = score;
                    bestAction = actions[i];
                }
            }

            if (bestScore > 0)
            {
                return new[] { bestAction };
            }

            return NextController.GetNextActions(robot).Take(1);
        }
    }
}