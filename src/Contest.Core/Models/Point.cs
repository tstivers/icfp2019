using System;

namespace Contest.Core.Models
{
    public struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public Point Translate(Direction facing)
        {
            switch (facing)
            {
                case Direction.Up: return new Point(X, Y + 1);
                case Direction.Down: return new Point(X, Y - 1);
                case Direction.Right: return new Point(X + 1, Y);
                case Direction.Left: return new Point(X - 1, Y);
            }

            throw new ArgumentException();
        }

        public Point Up()
        {
            return Translate(Direction.Up);
        }

        public Point Down()
        {
            return Translate(Direction.Down);
        }

        public Point Left()
        {
            return Translate(Direction.Left);
        }

        public Point Right()
        {
            return Translate(Direction.Right);
        }

        public Point Translate(Point p)
        {
            return new Point(X + p.X, Y + p.Y);
        }
    }
}