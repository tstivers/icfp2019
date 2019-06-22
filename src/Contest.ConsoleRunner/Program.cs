using Contest.Controllers.RobotControllers;
using Contest.Core.Loaders;
using Contest.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contest.ConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Arguments a = new Arguments();
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            parser.ExtractArgumentAttributes(a);
            parser.ParseCommandLine(args);

            if (!Directory.Exists(a.ProblemsFolder))
                throw new ArgumentException($"Problems folder {a.ProblemsFolder} does not exist");

            if (!Directory.Exists(a.SolutionsFolder))
                throw new ArgumentException($"Solutions folder {a.SolutionsFolder} does not exist");

            if (!Directory.Exists(a.CacheFolder))
                throw new ArgumentException($"Solutions folder {a.CacheFolder} does not exist");

            var problems = Directory.GetFiles(a.ProblemsFolder, "*.desc");

            var existingSolutions = Directory.GetFiles(a.SolutionsFolder, "*.sol");

            Parallel.ForEach(problems, new ParallelOptions { MaxDegreeOfParallelism = 8 }, p =>
            {
                var problem = ProblemLoader.LoadProblem(p, a.CacheFolder);
                Console.WriteLine($"Working on {problem.Name}");

                var controller = new ScoreTurnActionsController(problem, new IslandFinderController(problem, new ScoreSingleActionsController(problem, new DijkstraController(problem))));
                var done = false;

                do
                {
                    foreach (var robot in problem.Robots)
                    {
                        var actions = controller.GetNextActions(robot);
                        if (actions.Count() == 1 && actions.First() is RobotDoneAction)
                        {
                            var solutionFile = Path.Combine(a.SolutionsFolder, problem.Name + ".sol");
                            File.WriteAllText(solutionFile, problem.Solution);

                            Console.WriteLine($"Done with {problem.Name}");
                            done = true;
                            break;
                        }

                        if (!done)
                            problem.ProcessAction(robot, actions);
                    }

                    if (done)
                        break;
                } while (true);
            });
        }
    }
}