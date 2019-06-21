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

        public static void Main()
        {
            var problemsPath = ProblemsFinder.FindProblemsFolderPath();
            problem = ProblemLoader.LoadProblem(Path.Combine(problemsPath, "prob-130.desc"));

            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = problem.Map.Width;
            settings.Height = problem.Map.Height;
            settings.Scale = 0.5f;
            settings.Title = "RLNET Sample";
            //settings.WindowBorder = RLWindowBorder.Resizable;
            //settings.ResizeType = RLResizeType.ResizeCells;
            //settings.StartWindowState = RLWindowState.Normal;

            rootConsole = new RLRootConsole(settings);
            rootConsole.Render += rootConsole_Render;
            rootConsole.Update += rootConsole_Update;

            rootConsole.Run();
        }

        private static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Escape)
                    rootConsole.Close();
            }

            if (rootConsole.Mouse.GetLeftClick())
            {
                Console.WriteLine($"Mouse: {rootConsole.Mouse.X}, {problem.Map.Height - rootConsole.Mouse.Y - 1}");
            }
        }

        private static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            for (int y = 0; y < problem.Map.Height; y++)
                for (int x = 0; x < problem.Map.Width; x++)
                    rootConsole.SetChar(x, problem.Map.Height - y - 1, (char)problem.Map.Cells[y][x]);

            rootConsole.Draw();
        }
    }
}