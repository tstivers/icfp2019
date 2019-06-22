using Contest.Controllers.PathFinders;
using Contest.Controllers.TargetSelectors;
using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public class SimplestController
    {
        public Problem Problem { get; }

        public SimplestController(Problem problem)
        {
            Problem = problem;
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            // find the closest unwrapped cells
            var finder = new SimpleTargetSelector(Problem.Map);

            var targets = finder.GetPotentialTargets(robot.Position, 1);

            // sort them by moves
            var bestScore = int.MaxValue;
            Queue<RobotAction> bestRoute = null;

            if (targets.Count == 0)
                return new[] { RobotAction.Done };

            var rf = new AStarRouteFinder(Problem.Map);

            foreach (var p in targets)
            {
                var route = rf.GetRouteTo(robot.Position, p);

                if (route.Count <= bestScore)
                {
                    bestScore = route.Count;
                    bestRoute = route;
                    robot.Target = p;
                }
            }

            if (robot.PriorTarget != null && robot.Target != robot.PriorTarget)
            {
                var priorRoute = rf.GetRouteTo(robot.Position, robot.PriorTarget.Value);
                if (priorRoute.Count != 0)
                {
                    if (priorRoute.Count <= bestScore)
                    {
                        return priorRoute;
                    }
                }
            }

            robot.PriorTarget = robot.Target;
            return bestRoute;
        }
    }
}