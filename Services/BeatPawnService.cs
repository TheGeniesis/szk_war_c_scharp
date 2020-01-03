using Checkers.Adapters;
using Checkers.Calculators;
using Checkers.Configs;
using Checkers.Enums;
using System;
using System.Collections.Generic;

namespace Checkers.Services
{
    class BeatPawnService
    {
        private readonly MoveCalculator MoveCalculator;
        public BeatPawnService()
        {
            MoveCalculator = new MoveCalculator();
        }
        public bool TryToBeatPawn(PawnAdapter origin, PawnAdapter destination, List<PawnAdapter> bluePawns, List<PawnAdapter> redPawns)
        {
            int movedRows = MoveCalculator.CalculateMovedRows(origin, destination);
            int movedColumns = MoveCalculator.CalculateMovedColumns(origin, destination);
            if (RuleConfig.moveTwoRows == movedRows && RuleConfig.moveTwoColumns == Math.Abs(movedColumns))
            {
                AverageCalculator averageCalculator = new AverageCalculator();
                int skippedColumn = averageCalculator.CalculateAverage(destination.Column(), origin.Column());
                int skippedRow = averageCalculator.CalculateAverage(destination.Row(), origin.Row());
                List<PawnAdapter> oppositeSide = origin.Player() == PlayerEnum.Red ? bluePawns : redPawns;
                foreach (PawnAdapter oppositePawnAdapter in oppositeSide)
                {
                    if (oppositePawnAdapter.Row() == skippedRow && oppositePawnAdapter.Column() == skippedColumn)
                    {
                        oppositePawnAdapter.Hide();
                        return true;

                    }
                }
            }
            return false;
        }
    }
}
