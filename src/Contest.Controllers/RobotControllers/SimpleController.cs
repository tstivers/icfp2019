using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public class SimpleController
    {
        public Problem Problem { get; }
        public DijkstraController SimplestController { get; }

        public SimpleController(Problem problem)
        {
            Problem = problem;
            SimplestController = new DijkstraController(problem);
        }

        public IEnumerable<RobotAction> GetNextActions()
        {
            Problem.Targets = null;
            Problem.Target = null;

            // get scores for all actions
            var actions = new[]
            {
                /*RobotAction.Up, RobotAction.Down, RobotAction.Left, RobotAction.Right,*/ RobotAction.TurnLeft,
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
                return new[] { bestAction };
            }

            return SimplestController.GetNextActions();
        }
    }
}