using Contest.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Contest.Core.Loaders
{
    public static class ProblemLoader
    {
        public static Problem LoadProblem(string filename)
        {
            var p = new Problem();

            var pt = File.ReadAllText(filename);

            p.ProblemText = pt;

            var sectionsMatch = Regex.Match(pt, "^(.*?)#(.*?)#(.*?)#(.*?)$");

            var mapPolies = ParsePoints(sectionsMatch.Groups[1].Value);
            var startPos = ParsePoints(sectionsMatch.Groups[2].Value)[0];
            var obstacles = ParseObstacles(sectionsMatch.Groups[3].Value);
            var boosters = ParseBoosters(sectionsMatch.Groups[4].Value);

            var maxX = mapPolies.Max(x => x.X);
            var maxY = mapPolies.Max(x => x.Y);

            p.Map = new Map(maxX, maxY);

            p.Map.FillPoly(mapPolies, Map.CellType.Empty);

            foreach (var o in obstacles)
            {
                p.Map.FillPoly(o, Map.CellType.Wall);
            }

            return p;
        }

        public static List<Point> ParsePoints(string str)
        {
            var p = new List<Point>();

            foreach (Match m in Regex.Matches(str, @"\((\d+),(\d+)\)[, ]?"))
            {
                int x = int.Parse(m.Groups[1].Value);
                int y = int.Parse(m.Groups[2].Value);
                p.Add(new Point(x, y));
            }

            return p;
        }

        public static List<List<Point>> ParseObstacles(string str)
        {
            var o = new List<List<Point>>();

            foreach (Match m in Regex.Matches(str, @"(.*?);"))
            {
                var p = ParsePoints(m.Groups[1].Value);
                o.Add(p);
            }

            return o;
        }

        public static List<Booster> ParseBoosters(string str)
        {
            var b = new List<Booster>();

            foreach (Match m in Regex.Matches(str, @"([BFLXR])\((\d+),(\d+)\)[;]?"))
            {
                char type = m.Groups[1].Value[0];
                int x = int.Parse(m.Groups[2].Value);
                int y = int.Parse(m.Groups[3].Value);
                b.Add(new Booster(x, y, type));
            }

            return b;
        }
    }
}