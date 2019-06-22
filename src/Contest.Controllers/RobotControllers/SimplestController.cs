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

        public IEnumerable<RobotAction> GetNextAction()
        {
            // find the closest unwrapped cells
            var finder = new SimpleTargetSelector(Problem.Map);

            var targets = finder.GetPotentialTargets(Problem.Robot.Position, 1);

            // sort them by moves

            Point? target = null;
            if (targets.Count > 1)
            {
                // pick the first one duh
                target = targets[0];
            }
            else if (targets.Count == 1)
                target = targets[0];
            else if (targets.Count == 0)
                return new[] { RobotAction.Done };

            // get a path to it
            var rf = new AStarRouteFinder(Problem.Map);

            var route = rf.GetRouteTo(Problem.Robot.Position, target.Value);

            return route;
        }
    }
}