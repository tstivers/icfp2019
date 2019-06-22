using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class SimpleController
    {
        public Problem Problem { get; }
        public SimplestController SimplestController { get; }

        public SimpleController(Problem problem)
        {
            Problem = problem;
            SimplestController = new SimplestController(problem);
        }

        public IEnumerable<RobotAction> GetNextActions()
        {
            Problem.Targets = null;
            Problem.Target = null;

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
                var score = Problem.ScoreAction(actions[i]);
                if (score > 1 && score >= bestScore)
                {
                    bestScore = score;
                    bestAction = actions[i];
                }
            }

            if (bestScore > 0)
            {
                SimplestController.PriorTarget = null;
                return new[] { bestAction };
            }

            return SimplestController.GetNextActions().Take(1);
        }
    }
}