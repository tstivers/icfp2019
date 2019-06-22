using Contest.Core.Models;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Controllers.PathFinders
{
    public static class AStarPathFinder
    {
        public static Queue<RobotAction> GetRouteTo(Point start, Point goal, Map map, int maxDistance = int.MaxValue)
        {
            if (start == goal)
                return new Queue<RobotAction>();

            var closed_set = new HashSet<Point>();
            var came_from = new Dictionary<Point, Point>();
            var g_score = new Dictionary<Point, float> { { start, 0 } };
            var open_set = new SimplePriorityQueue<Point, float>();

            open_set.Enqueue(start, GetDistance(start, goal));

            while (open_set.Count != 0)
            {
                var current = open_set.Dequeue();

                if (current == goal)
                    return ReconstructPath(came_from, start, goal);

                closed_set.Add(current);

                var tentative_g_score = g_score[current] + 1;

                if (tentative_g_score > maxDistance)
                    continue;

                foreach (var neighbor in map.Neighbors(current))
                {
                    if (closed_set.Contains(neighbor))
                        continue;

                    if (!open_set.Contains(neighbor))
                    {
                        open_set.Enqueue(neighbor, tentative_g_score + GetDistance(neighbor, goal));
                    }
                    else if (tentative_g_score >= g_score[neighbor])
                        continue;

                    came_from[neighbor] = current;
                    g_score[neighbor] = tentative_g_score;
                    open_set.UpdatePriority(neighbor, tentative_g_score + GetDistance(neighbor, goal));
                }
            }

            return null;
        }

        private static Queue<RobotAction> ReconstructPath(Dictionary<Point, Point> came_from, Point start, Point current)
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

        private static float GetDistance(Point start, Point goal)
        {
            return
                (float)
                (Math.Sqrt(Math.Pow(Math.Abs((float)start.X - goal.X), 2) +
                           Math.Pow(Math.Abs((float)start.Y - goal.Y), 2)));
        }
    }
}