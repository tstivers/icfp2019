using System;

namespace Contest.Core.Extensions
{
    public static class MathExtensions
    {
        public static int Clamp(int x, int min, int max)
        {
            return Math.Min(Math.Max(x, min), max);
        }
    }
}