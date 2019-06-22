using Contest.Controllers.PathFinders;
using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public class DijkstraController
    {
        public Problem Problem { get; }

        public DijkstraController(Problem problem)
        {
            Problem = problem;
        }

        public IEnumerable<RobotAction> GetNextActions()
        {
            var actions = DijkstraPathfinder.ClosestUnwrappedCell(Problem.Robot.Position, Problem.Map);

            if (actions == null)
                return new[] { RobotAction.Done };

            return actions;
        }
    }
}