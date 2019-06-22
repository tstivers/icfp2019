using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Models
{
    public class Map
    {
        public enum CellType
        {
            Wall = '#',
            Empty = ' ',
            Wrapped = '+'
        }

        public CellType[][] Cells { get; }
        public int Width { get; }
        public int Height { get; }

        public Map(int x, int y)
        {
            Width = x;
            Height = y;

            Cells = new CellType[Height][];
            for (var i = 0; i < Height; i++)
                Cells[i] = new CellType[Width];

            for (y = 0; y < Height; y++)
                for (x = 0; x < Width; x++)
                    Cells[y][x] = CellType.Wall;
        }

        public Map(CellType[][] cells)
        {
            Cells = cells;

            Width = cells[0].Length;
            Height = cells.Length;
        }

        public void FillPoly(List<Point> points, CellType cellType)
        {
            if (points.Count == 0)
                return;

            var lines = new List<Tuple<Point, Point>>();

            var prev = points[0];
            foreach (var p in points.Skip(1))
            {
                lines.Add(Tuple.Create(prev, p));
                prev = p;
            }

            lines.Add(Tuple.Create(points.Last(), points.First()));

            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    var icount = Intersections(x, y, lines);
                    if (icount % 2 == 1)
                        Cells[y][x] = cellType;
                }
        }

        private int Intersections(int sx, int sy, List<Tuple<Point, Point>> lines)
        {
            var x = sx + 0.5;
            var y = sy + 0.5;

            var count = 0;

            foreach (var line in lines)
            {
                if (line.Item1.X != line.Item2.X) // horizontal line, cannot intersesct
                    continue;

                if (line.Item1.X > x) // line to the right, cannot intersect
                    continue;

                // one y has to be less, one has to be greater
                if (Math.Min(line.Item1.Y, line.Item2.Y) < y &&
                    Math.Max(line.Item1.Y, line.Item2.Y) > y)
                    count++;
            }

            return count;
        }

        public CellType CellAt(Point p)
        {
            if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height)
                return CellType.Wall;

            return Cells[p.Y][p.X];
        }

        public CellType CellAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return CellType.Wall;

            return Cells[y][x];
        }

        public IEnumerable<Point> Neighbors(Point pos)
        {
            var np = new[] { pos.Up(), pos.Down(), pos.Left(), pos.Right() };
            return np.Where(x => CellAt(x) != CellType.Wall);
        }

        public IEnumerable<Point> Neighbors(Point pos, bool unwrappedBlocks)
        {
            var np = new[] { pos.Up(), pos.Down(), pos.Left(), pos.Right() };
            var neighbors = np.Where(x => CellAt(x) != CellType.Wall);
            if (unwrappedBlocks)
                neighbors = neighbors.Where(x => CellAt(x) != CellType.Wrapped);
            return neighbors;
        }

        public void SetCell(Point p, CellType type)
        {
            if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height)
                return;

            Cells[p.Y][p.X] = type;
        }
    }
}