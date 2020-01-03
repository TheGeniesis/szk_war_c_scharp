namespace Checkers.Configs
{
    public class RuleConfig
    {
        public const int firstRow = 1;
        public const int lastRow = 8;
        public const int moveOneRow = 1;
        public const int moveTwoRows = moveOneRow * 2;
        public const int moveBackTwoRows = moveTwoRows * -1;

        public const int firstColumn = 1;
        public const int lastColumn = 8;
        public const int moveOneColumn = 1;
        public const int moveTwoColumns = moveOneColumn * 2;
    }
}
