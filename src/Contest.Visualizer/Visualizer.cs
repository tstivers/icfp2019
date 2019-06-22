using Contest.Controllers.RobotControllers;
using Contest.Core.Helpers;
using Contest.Core.Loaders;
using Contest.Core.Models;
using RLNET;
using System;
using System.IO;
using System.Linq;

namespace Contest.Visualizer
{
    public class Visualizer
    {
        public static RLRootConsole rootConsole;
        public static Problem problem;
        public static IRobotController controller;

        public static void Main()
        {
            ResetSimulation();

            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = problem.Map.Width;
            settings.Height = problem.Map.Height;
            settings.Scale = 1.0f;
            settings.Title = "RLNET Sample";
            //settings.WindowBorder = RLWindowBorder.Resizable;
            //settings.ResizeType = RLResizeType.ResizeCells;
            //settings.StartWindowState = RLWindowState.Normal;

            rootConsole = new RLRootConsole(settings);
            rootConsole.Render += rootConsole_Render;
            rootConsole.Update += rootConsole_Update;

            rootConsole.Run();
        }

        private static bool start = false;

        private static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Escape)
                    rootConsole.Close();

                if (keyPress.Key == RLKey.Space)
                {
                    start = !start;
                }

                if (keyPress.Key == RLKey.R)
                {
                    ResetSimulation();
                }

                if (keyPress.Key == RLKey.S)
                {
                    StepSimulation();
                }
            }

            if (rootConsole.Mouse.GetLeftClick())
            {
                Console.WriteLine($"Mouse: {rootConsole.Mouse.X}, {problem.Map.Height - rootConsole.Mouse.Y - 1}");
            }

            if (start)
            {
                StepSimulation();
            }
        }

        private static void StepSimulation()
        {
            foreach (var robot in problem.Robots)
            {
                var actions = controller.GetNextActions(robot);

                if (actions.First() is RobotDoneAction)
                {
                    start = false;
                    Console.WriteLine($"Moves: {problem.Solution.Length}");
                }
                else
                    problem.ProcessAction(robot, actions);
            }
        }

        public static void ResetSimulation()
        {
            var problemsPath = ProblemsFinder.FindProblemsFolderPath();
            problem = ProblemLoader.LoadProblem(Path.Combine(problemsPath, "prob-021.desc"), null);
            controller = new CheckTurnsController(problem, new IslandFinderController(problem, new ScoreSingleActionsController(problem, new DijkstraController(problem))));
            //controller = new IslandFinderController(problem, new ScoreSingleActionsController(problem, new DijkstraController(problem)));
            //controller = new ScoreSingleActionsController(problem, new DijkstraController(problem));
            start = false;
        }

        private static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            Point p = new Point(0, 0);

            for (p.Y = 0; p.Y < problem.Map.Height; p.Y++)
                for (p.X = 0; p.X < problem.Map.Width; p.X++)
                {
                    var value = problem.Map.CellAt(p);
                    if (value != Map.CellType.Wrapped)
                    {
                        rootConsole.SetChar(p, (char)value);
                        if (value == Map.CellType.Wall)
                            rootConsole.SetColor(p, RLColor.LightGray);
                    }
                    else
                    {
                        rootConsole.SetChar(p, ' ');
                        rootConsole.SetBackColor(p, RLColor.Yellow);
                    }
                }

            foreach (var r in problem.Robots)
            {
                rootConsole.SetChar(r.Position, 'R');
                rootConsole.SetColor(r.Position, RLColor.LightGreen);

                if (r.Target.HasValue)
                    rootConsole.SetBackColor(r.Target.Value, RLColor.Blue);

                if (r.Targets != null)
                    foreach (var t in r.Targets)
                        rootConsole.SetBackColor(t, RLColor.Red);

                foreach (var arm in r.Arms)
                {
                    rootConsole.SetBackColor(r.Position.Translate(arm), RLColor.LightGreen);
                }
            }

            rootConsole.Draw();
        }
    }
}