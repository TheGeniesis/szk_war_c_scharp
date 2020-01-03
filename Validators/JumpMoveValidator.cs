using Checkers.Adapters;
using Checkers.Calculators;
using Checkers.Configs;
using Checkers.Enums;
using System;
using System.Collections.Generic;

namespace Checkers.Validators
{
    class JumpMoveValidator
    {
        private readonly AverageCalculator AverageCalculator;
        private readonly MoveCalculator MoveCalculator;
        public JumpMoveValidator() 
        {
            AverageCalculator = new AverageCalculator();
            MoveCalculator = new MoveCalculator();
        }
        public bool IsValid(PawnAdapter origin, PawnAdapter destination, List<PawnAdapter> bluePawns, List<PawnAdapter> redPawns)
        {
            int movedRows = MoveCalculator.CalculateMovedRows(origin, destination);
            int movedColumns = MoveCalculator.CalculateMovedColumns(origin, destination);
            if (RuleConfig.moveTwoRows != movedRows || RuleConfig.moveTwoColumns != Math.Abs(movedColumns))
            {
                return false;
            }
             
            int skippedColumn = AverageCalculator.CalculateAverage(destination.Column(), origin.Column());
            int skippedRow = AverageCalculator.CalculateAverage(destination.Row(), origin.Row());
            List<PawnAdapter> oppositeSide = origin.Player() == PlayerEnum.Red ? bluePawns : redPawns;
            foreach (PawnAdapter oppositePawnAdapter in oppositeSide)
            {
                if (oppositePawnAdapter.Row() == skippedRow && oppositePawnAdapter.Column() == skippedColumn)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
