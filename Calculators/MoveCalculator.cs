using Checkers.Adapters;
using Checkers.Enums;
using System;

namespace Checkers.Calculators
{
    class MoveCalculator
    {
        public int CalculateMovedColumns(PawnAdapter origin, PawnAdapter destination)
        {
            return origin.Column() - destination.Column();
        }
        public int CalculateMovedRows(PawnAdapter origin, PawnAdapter destination)
        {
            int movedRows = origin.Row() - destination.Row();
            movedRows = origin.Player() == PlayerEnum.Red ? movedRows : (movedRows * -1);

            return origin.Type() == PawnTypeEnum.Jumper ? Math.Abs(movedRows) : movedRows;
        }
    }
}
