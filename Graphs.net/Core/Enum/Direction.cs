namespace Graphs.Core.Enum
{
    public enum Direction
    {
        NotDirected = 0,
        LeftToRight = 1,
        RightToLeft = 2
    }

    public static class DirectionExtensions
    {
        public static Direction GetOpposedTo(Direction direction)
        {
            return direction == Direction.LeftToRight ? Direction.RightToLeft : Direction.LeftToRight;
        }
    }
}