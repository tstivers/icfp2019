namespace Contest.Core.Models
{
    public struct Booster
    {
        public readonly int X;
        public readonly int Y;
        public readonly char Type;

        public Booster(int x, int y, char type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type} {{{X}, {Y}}}";
        }
    }
}