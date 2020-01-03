using Checkers.Adapters;
using Checkers.Calculators;
using Checkers.Configs;
using Checkers.Enums;
using System;

namespace Checkers.Validators
{
    class BasicMoveValidator
    {
        private readonly MoveCalculator MoveCalculator;
        public BasicMoveValidator() 
        {
            MoveCalculator = new MoveCalculator();
        }

        public bool IsValid(PawnAdapter origin, PawnAdapter destination)
        {
            int movedRows = MoveCalculator.CalculateMovedRows(origin, destination);
            movedRows = origin.Type() == PawnTypeEnum.Jumper ? Math.Abs(movedRows) : movedRows;
            int movedColumns = MoveCalculator.CalculateMovedColumns(origin, destination);

            return (movedRows == RuleConfig.moveOneRow && Math.Abs(movedColumns) == RuleConfig.moveOneColumn);
        }
    }
}
