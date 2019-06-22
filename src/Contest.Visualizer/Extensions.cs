using Contest.Core.Models;
using RLNET;

namespace Contest.Visualizer
{
    public static class RLRootConsoleExtensions
    {
        public static void SetBackColor(this RLRootConsole console, Point p, RLColor color)
        {
            console.SetBackColor(p.X, console.Height - p.Y - 1, color);
        }

        public static void SetChar(this RLRootConsole console, Point p, int c)
        {
            console.SetChar(p.X, console.Height - p.Y - 1, c);
        }

        public static void SetColor(this RLRootConsole console, Point p, RLColor color)
        {
            console.SetColor(p.X, console.Height - p.Y - 1, color);
        }
    }
}