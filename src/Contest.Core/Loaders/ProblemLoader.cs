using Contest.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Contest.Core.Loaders
{
    public static class ProblemLoader
    {
        public static Problem LoadProblem(string filename, string cacheFolder)
        {
            var p = new Problem();

            var pt = File.ReadAllText(filename);

            p.Name = Path.GetFileNameWithoutExtension(filename);

            p.ProblemText = pt;

            var sectionsMatch = Regex.Match(pt, "^(.*?)#(.*?)#(.*?)#(.*?)$");
            var startPos = ParsePoints(sectionsMatch.Groups[2].Value)[0];
            var boosters = ParseBoosters(sectionsMatch.Groups[4].Value);

            p.Robots.Add(new Robot
            {
                Position = startPos,
                Facing = Direction.Right
            });

            var cacheFile = cacheFolder == null ? null : Path.Combine(cacheFolder, Path.GetFileNameWithoutExtension(filename) + ".cache");

            if (cacheFile == null || !File.Exists(cacheFile))
            {
                var mapPolies = ParsePoints(sectionsMatch.Groups[1].Value);
                var obstacles = ParseObstacles(sectionsMatch.Groups[3].Value);

                var maxX = mapPolies.Max(x => x.X);
                var maxY = mapPolies.Max(x => x.Y);

                p.Map = new Map(maxX, maxY);

                p.Map.FillPoly(mapPolies, Map.CellType.Empty);

                foreach (var o in obstacles)
                {
                    p.Map.FillPoly(o, Map.CellType.Wall);
                }

                if (cacheFile != null)
                    SaveCells(cacheFile, p.Map.Cells);
            }
            else
            {
                p.Map = new Map(LoadCells(cacheFile));
            }

            p.WrapArms(p.Robots[0]);

            return p;
        }

        public static Map.CellType[][] LoadCells(string filename)
        {
            var f = new BinaryFormatter();
            return (Map.CellType[][])f.Deserialize(File.OpenRead(filename));
        }

        public static void SaveCells(string filename, Map.CellType[][] cells)
        {
            var f = new BinaryFormatter();
            f.Serialize(File.OpenWrite(filename), cells);
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

            foreach (var ob in str.Split(';'))
            {
                var p = ParsePoints(ob);
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