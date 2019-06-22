using Contest.Controllers.PathFinders;
using Contest.Controllers.TargetSelectors;
using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public class SimplestController
    {
        public Problem Problem { get; }

        public Point? PriorTarget { get; set; }

        public SimplestController(Problem problem)
        {
            Problem = problem;
        }

        public IEnumerable<RobotAction> GetNextActions()
        {
            // find the closest unwrapped cells
            var finder = new SimpleTargetSelector(Problem.Map);

            var targets = finder.GetPotentialTargets(Problem.Robot.Position, 1);

            Problem.Targets = targets;

            // sort them by moves
            var bestScore = int.MaxValue;
            Queue<RobotAction> bestRoute = null;

            if (targets.Count == 0)
                return new[] { RobotAction.Done };

            var rf = new AStarRouteFinder(Problem.Map);

            foreach (var p in targets)
            {
                var route = rf.GetRouteTo(Problem.Robot.Position, p);

                if (route.Count <= bestScore)
                {
                    bestScore = route.Count;
                    bestRoute = route;
                    Problem.Target = p;
                }
            }

            if (PriorTarget != null && Problem.Target != PriorTarget)
            {
                var priorRoute = rf.GetRouteTo(Problem.Robot.Position, PriorTarget.Value);
                if (priorRoute.Count != 0)
                {
                    if (priorRoute.Count <= bestScore)
                    {
                        return priorRoute;
                    }
                }
            }

            PriorTarget = Problem.Target;
            return bestRoute;
        }
    }
}