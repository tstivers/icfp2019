using Contest.Core.Models;
using System;

namespace Contest.Core.Extensions
{
    public static class DirectionExtensions
    {
        public static Direction Rotate(this Direction Facing, Direction d)
        {
            if (d == Direction.Left)
            {
                switch (Facing)
                {
                    case Direction.Up:
                        return Direction.Left;

                    case Direction.Down:
                        return Direction.Right;

                    case Direction.Left:
                        return Direction.Down;

                    case Direction.Right:
                        return Direction.Up;
                }
            }
            else
            {
                switch (Facing)
                {
                    case Direction.Up:
                        return Direction.Right;

                    case Direction.Down:
                        return Direction.Left;

                    case Direction.Left:
                        return Direction.Up;

                    case Direction.Right:
                        return Direction.Down;
                }
            }

            throw new ArgumentException();
        }
    }
}