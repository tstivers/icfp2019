using Contest.Controllers.PathFinders;
using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    // search a 5x5 area around robot, find islands of < 10 empty cells less than 10 moves away
    public class IslandFinderController : IRobotController
    {
        public IslandFinderController(Problem problem, IRobotController nextController)
        {
            Problem = problem;
            NextController = nextController;
        }

        public Problem Problem { get; }
        public IRobotController NextController { get; }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            robot.Targets = null;
            robot.Target = null;

            // gimme all the empty cells within x moves
            var empties = DijkstraPathfinder.FindUnwrappedCellsWithin(robot.Position, Problem.Map, 10, false);

            if (empties.Count == 0)
                return NextController.GetNextActions(robot);

            var islands = new Dictionary<Point, HashSet<Point>>();
            var notIslands = new HashSet<Point>();

            foreach (var t in empties)
            {
                // point is not in an island cluster
                if (notIslands.Contains(t))
                    continue;

                // point is already in an island
                bool found = false;
                foreach (var island in islands.Values)
                    if (island.Contains(t))
                    {
                        found = true;
                        break;
                    }

                if (found)
                    continue;

                var np = DijkstraPathfinder.FindUnwrappedCellsWithin(t, Problem.Map, 100, true);
                if (np.Count > 50)
                    np.ToList().ForEach(x => notIslands.Add(x)); // not an island, mark 'em
                else
                {
                    // found a new island
                    islands.Add(t, new HashSet<Point>(np));
                }
            }

            // no islands, do something else
            if (islands.Count == 0)
                return NextController.GetNextActions(robot);

            // paint islands
            robot.Targets = new HashSet<Point>(islands.Values.SelectMany(x => x));

            // find the closest point in the smallest island
            var closestScore = int.MaxValue;
            IEnumerable<RobotAction> closestRoute = null;

            foreach (var target in islands.OrderBy(x => x.Value.Count).First().Value)
            {
                var result = AStarPathFinder.GetRouteTo(robot.Position, target, Problem.Map, closestScore);

                if (result != null && result.Count < closestScore)
                {
                    closestScore = result.Count;
                    closestRoute = result;
                    robot.Target = target;
                }
            }

            // go to there
            return closestRoute.Take(1);
        }
    }
}