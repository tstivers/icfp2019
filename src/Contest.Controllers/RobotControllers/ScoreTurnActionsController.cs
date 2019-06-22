using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class ScoreTurnActionsController : RobotControllerBase, IRobotController
    {
        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            var currentAction = NextController.GetNextActions(robot).First();
            var currentScore = Problem.ScoreAction(robot, currentAction);

            // get scores for turn actions
            var actions = new[]
            {
                RobotAction.TurnLeft, RobotAction.TurnRight
            };

            var bestScore = currentScore;
            var bestAction = currentAction;

            for (int i = 0; i < actions.Length; i++)
            {
                var score = Problem.ScoreAction(robot, actions[i]);
                if (score > 0 && score >= bestScore)
                {
                    bestScore = score;
                    bestAction = actions[i];
                }
            }

            return new[] { bestAction };
        }

        public ScoreTurnActionsController(Problem problem, IRobotController nextController) : base(problem, nextController)
        {
        }
    }
}