using C5;
using Contest.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.PathFinders
{
    public class AStarRouteFinder
    {
        private readonly System.Collections.Generic.HashSet<Point> _unreachable =
            new System.Collections.Generic.HashSet<Point>();

        public Map Map { get; }

        public AStarRouteFinder(Map map)
        {
            Map = map;
        }

        public Queue<RobotAction> GetRouteTo(Point start, Point goal)
        {
            if (start == goal)
                return new Queue<RobotAction>();

            if (_unreachable.Contains(goal))
                return null;

            var closed_set = new System.Collections.Generic.HashSet<Point>();
            var came_from = new Dictionary<Point, Point>();
            var g_score = new Dictionary<Point, float> { { start, 0 } };
            var f_score = new Dictionary<Point, float> { { start, GetDistance(start, goal) } };
            var current = start;
            IPriorityQueue<Point> open_set = new IntervalHeap<Point>(new PointComparer(f_score)) { start };
            var open_set_hash = new System.Collections.Generic.HashSet<Point>();

            while (!open_set.IsEmpty)
            {
                current = open_set.DeleteMin();
                if (current == goal)
                    return ReconstructPath(came_from, start, goal);
                closed_set.Add(current);

                foreach (var neighbor in Map.Neighbors(current))
                {
                    if (closed_set.Contains(neighbor))
                        continue;

                    var tentative_g_score = g_score[current] + MoveCost(current, neighbor);

                    if (!open_set_hash.Contains(neighbor) || tentative_g_score < g_score[neighbor])
                    {
                        g_score[neighbor] = tentative_g_score;
                        f_score[neighbor] = g_score[neighbor] + GetDistance(neighbor, goal);
                        open_set.Add(neighbor);
                        open_set_hash.Add(neighbor);
                        came_from[neighbor] = current;
                    }
                }
            }

            // fill out our unreachable points
            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    if (!closed_set.Contains(new Point(x, y)))
                        _unreachable.Add(new Point(x, y));
                }
            }

            return null;
        }

        private float MoveCost(Point current, Point neighbor)
        {
            return 1;
        }

        private Queue<RobotAction> ReconstructPath(Dictionary<Point, Point> came_from, Point start, Point current)
        {
            var path = new Queue<RobotAction>();

            while (current != start)
            {
                var prev = came_from[current];

                if (prev.Up() == current)
                    path.Enqueue(RobotAction.Up);
                else if (prev.Down() == current)
                    path.Enqueue(RobotAction.Down);
                else if (prev.Left() == current)
                    path.Enqueue(RobotAction.Left);
                else if (prev.Right() == current)
                    path.Enqueue(RobotAction.Right);

                current = prev;
            }

            return new Queue<RobotAction>(path.Reverse());
        }

        private float GetDistance(Point start, Point goal)
        {
            return
                (float)
                (Math.Sqrt(Math.Pow(Math.Abs((float)start.X - goal.X), 2) +
                           Math.Pow(Math.Abs((float)start.Y - goal.Y), 2)));
        }

        private class PointComparer : IComparer<Point>
        {
            private readonly Dictionary<Point, float> _f_scores;

            public PointComparer(Dictionary<Point, float> f_scores)
            {
                _f_scores = f_scores;
            }

            public int Compare(Point x, Point y)
            {
                return _f_scores[x].CompareTo(_f_scores[y]);
            }
        }
    }
}