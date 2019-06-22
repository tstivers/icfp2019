using Contest.Core.Models;
using System;
using System.Collections.Generic;

namespace Contest.Controllers.TargetSelectors
{
    public class SimpleTargetSelector
    {
        public SimpleTargetSelector(Map map)
        {
            Map = map;
        }

        public Map Map { get; }

        public List<Point> GetPotentialTargets(Point start, int desiredNumber)
        {
            int d = 1;

            List<Point> targets = GetTargets(start, d);

            // keep expanding outwards until we have {desiredNumber} targets
            while (targets.Count < desiredNumber && d <= Math.Max(Map.Width, Map.Height))
            {
                d++;
                targets.AddRange(GetTargets(start, d));
            }

            return targets;
        }

        public List<Point> GetTargets(Point start, int distance)
        {
            var targets = new List<Point>();

            // scan top
            if (start.Y + distance < Map.Height)
            {
                var startx = Math.Max(start.X - distance, 0);
                var endx = Math.Min(start.X + distance, Map.Width - 1);
                var y = start.Y + distance;
                for (int x = startx; x <= endx; x++)
                {
                    if (Map.Cells[y][x] == Map.CellType.Empty)
                    {
                        var p = new Point(x, y);
                        if (!targets.Contains(p))
                            targets.Add(p);
                    }
                }
            }

            // scan bottom
            if (start.Y - distance >= 0)
            {
                var startx = Math.Max(start.X - distance, 0);
                var endx = Math.Min(start.X + distance, Map.Width - 1);
                var y = start.Y - distance;
                for (int x = startx; x <= endx; x++)
                {
                    if (Map.Cells[y][x] == Map.CellType.Empty)
                    {
                        var p = new Point(x, y);
                        if (!targets.Contains(p))
                            targets.Add(p);
                    }
                }
            }

            // scan right
            if (start.X + distance < Map.Width)
            {
                var starty = Math.Min(start.Y + distance, Map.Height - 1);
                var endy = Math.Max(start.Y - distance, 0);
                var x = start.X + distance;

                for (int y = starty; y >= endy; y--)
                {
                    if (Map.Cells[y][x] == Map.CellType.Empty)
                    {
                        var p = new Point(x, y);
                        if (!targets.Contains(p))
                            targets.Add(p);
                    }
                }
            }

            // scan left
            if (start.X - distance >= 0)
            {
                var starty = Math.Min(start.Y + distance, Map.Height - 1);
                var endy = Math.Max(start.Y - distance, 0);
                var x = start.X - distance;

                for (int y = starty; y >= endy; y--)
                {
                    if (Map.Cells[y][x] == Map.CellType.Empty)
                    {
                        var p = new Point(x, y);
                        if (!targets.Contains(p))
                            targets.Add(p);
                    }
                }
            }

            return targets;
        }
    }
}