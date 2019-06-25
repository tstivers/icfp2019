using Contest.Controllers.PathFinders;
using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.RobotControllers
{
    public class SmartIslandController : RobotController, IRobotController
    {
        public SmartIslandController(Problem problem, IRobotController nextController) : base(problem, nextController)
        {
        }

        public IEnumerable<RobotAction> GetNextActions(Robot robot)
        {
            robot.Targets = null;
            robot.Target = null;

            // gimme all the empty cells within x moves
            var empties = DijkstraPathfinder.FindUnwrappedCellsWithin(robot.Position, Problem.Map, int.MaxValue, false);

            if (empties.Count == 0)
                return new[] { RobotAction.Done, };

            var islands = new List<HashSet<Point>>();

            foreach (var t in empties)
            {
                // point is already in an island
                bool found = false;
                foreach (var island in islands)
                    if (island.Contains(t))
                    {
                        found = true;
                        break;
                    }

                if (found)
                    continue;

                var np = DijkstraPathfinder.FindUnwrappedCellsWithin(t, Problem.Map, int.MaxValue, true);
                islands.Add(new HashSet<Point>(np));
            }

            // no islands, do something else
            if (islands.Count == 0)
                return NextController.GetNextActions(robot);

            // find smallest island
            var smallestIsland = islands.OrderBy(x => x.Count).First();
            robot.Targets = smallestIsland;

            // dij a path to it
            var route = DijkstraPathfinder.RouteToClosestCell(robot.Position, smallestIsland, Problem.Map);

            robot.Target = route.Item1;

            // go to there
            return route.Item2;
        }
    }
}