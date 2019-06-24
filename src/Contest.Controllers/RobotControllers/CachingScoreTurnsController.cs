using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class CachingScoreTurnActionsController : RobotController, IRobotController
    {
        public Queue<RobotAction> ActionsCache;

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            if (ActionsCache == null || ActionsCache.Count == 0)
                ActionsCache = new Queue<RobotAction>(NextController.GetNextActions(robot).ToList());

            var currentAction = ActionsCache.Dequeue();
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
                    ActionsCache.Clear();
                }
            }

            return new[] { bestAction };
        }

        public CachingScoreTurnActionsController(Problem problem, IRobotController nextController) : base(problem, nextController)
        {
        }
    }
}