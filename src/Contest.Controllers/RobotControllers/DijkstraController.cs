using Contest.Controllers.PathFinders;
using Contest.Core.Models;
using System.Collections.Generic;

namespace Contest.Controllers.RobotControllers
{
    public class DijkstraController : IRobotController
    {
        public Problem Problem { get; }

        public DijkstraController(Problem problem)
        {
            Problem = problem;
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            var result = DijkstraPathfinder.ClosestUnwrappedCell(robot.Position, Problem.Map);

            if (result == null)
                return new[] { RobotAction.Done };

            return result.Item2;
        }
    }
}