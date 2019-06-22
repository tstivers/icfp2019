using Contest.Controllers.RobotControllers;
using Contest.Core.Helpers;
using Contest.Core.Loaders;
using Contest.Core.Models;
using RLNET;
using System;
using System.IO;

namespace Contest.Visualizer
{
    public class Program
    {
        public static RLRootConsole rootConsole;
        public static Problem problem;
        public static SimpleController controller;

        public static void Main()
        {
            var problemsPath = ProblemsFinder.FindProblemsFolderPath();
            problem = ProblemLoader.LoadProblem(Path.Combine(problemsPath, "prob-100.desc"), null);
            controller = new SimpleController(problem);

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
                    var problemsPath = ProblemsFinder.FindProblemsFolderPath();
                    problem = ProblemLoader.LoadProblem(Path.Combine(problemsPath, "prob-026.desc"), null);
                    controller = new SimpleController(problem);
                    start = false;
                }

                if (keyPress.Key == RLKey.S)
                {
                    var actions = controller.GetNextActions();
                    problem.ProcessAction(actions);
                }
            }

            if (rootConsole.Mouse.GetLeftClick())
            {
                Console.WriteLine($"Mouse: {rootConsole.Mouse.X}, {problem.Map.Height - rootConsole.Mouse.Y - 1}");
            }

            if (start)
            {
                var actions = controller.GetNextActions();
                problem.ProcessAction(actions);
            }
        }

        private static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            for (int y = 0; y < problem.Map.Height; y++)
                for (int x = 0; x < problem.Map.Width; x++)
                    rootConsole.SetChar(x, problem.Map.Height - y - 1, (char)problem.Map.Cells[y][x]);

            rootConsole.SetChar(problem.Robot.Position.X, problem.Map.Height - problem.Robot.Position.Y - 1, 'R');
            rootConsole.SetColor(problem.Robot.Position.X, problem.Map.Height - problem.Robot.Position.Y - 1, RLColor.LightGreen);
            rootConsole.SetBackColor(problem.Robot.Front.X, problem.Map.Height - problem.Robot.Front.Y - 1, RLColor.LightGreen);

            if (problem.Targets != null)
                foreach (var target in problem.Targets)
                {
                    rootConsole.SetBackColor(target, RLColor.LightBlue);
                }

            if (problem.Target.HasValue)
                rootConsole.SetBackColor(problem.Target.Value, RLColor.Blue);

            rootConsole.Draw();
        }
    }
}