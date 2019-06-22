using Contest.Core.Models;
using Priority_Queue;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.PathFinders
{
    public sealed class DijkstraPathfinder
    {
        public static Queue<RobotAction> ClosestUnwrappedCell(Point start, Map map)
        {
            var dist = new Dictionary<Point, int>();
            var prev = new Dictionary<Point, Point>();
            var Q = new SimplePriorityQueue<Point, int>();

            dist[start] = 0;

            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)
                {
                    var p = new Point(x, y);
                    if (p != start)
                        dist[p] = int.MaxValue / 2;
                    Q.Enqueue(p, dist[p]);
                }

            while (Q.Count > 0)
            {
                var u = Q.Dequeue();
                if (map.CellAt(u) == Map.CellType.Empty)
                {
                    return GetActions(start, u, prev);
                }

                foreach (var v in map.Neighbors(u))
                {
                    var alt = dist[u] + 1;

                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                        Q.UpdatePriority(v, alt);
                    }
                }
            };

            return null;
        }

        private static Queue<RobotAction> GetActions(Point start, Point target, Dictionary<Point, Point> prev)
        {
            var path = new Queue<RobotAction>();

            while (target != start)
            {
                var pos = prev[target];

                if (pos.Up() == target)
                    path.Enqueue(RobotAction.Up);
                else if (pos.Down() == target)
                    path.Enqueue(RobotAction.Down);
                else if (pos.Left() == target)
                    path.Enqueue(RobotAction.Left);
                else if (pos.Right() == target)
                    path.Enqueue(RobotAction.Right);

                target = pos;
            }

            return new Queue<RobotAction>(path.Reverse());
        }
    }
}