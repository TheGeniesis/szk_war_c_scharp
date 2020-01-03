using Checkers.Adapters;
using Checkers.Calculators;
using Checkers.Configs;
using Checkers.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Checkers.Validators
{
    class ExtraMoveValidator
    {
        private readonly AverageCalculator AverageCalculator;
        public ExtraMoveValidator()
        {
            AverageCalculator = new AverageCalculator();
        }

        public bool IsValid(PawnAdapter selectedPawn, List<PawnAdapter> bluePawns, List<PawnAdapter> redPawns)
        {
            List<PawnAdapter> oponentPlayer = selectedPawn.Player() == PlayerEnum.Red ? bluePawns : redPawns;
            List<Point> positions = new List<Point>();
            int nextPosition = selectedPawn.Player() == PlayerEnum.Red ? RuleConfig.moveBackTwoRows : RuleConfig.moveTwoRows;

            positions.Add(new Point(selectedPawn.Column() + RuleConfig.moveTwoColumns, selectedPawn.Row() + nextPosition));
            positions.Add(new Point(selectedPawn.Column() - RuleConfig.moveTwoColumns, selectedPawn.Row() + nextPosition));

            if (selectedPawn.Type() == PawnTypeEnum.Jumper)
            {
                positions.Add(new Point(selectedPawn.Column() + RuleConfig.moveTwoColumns, selectedPawn.Row() - nextPosition));
                positions.Add(new Point(selectedPawn.Column() - RuleConfig.moveTwoColumns, selectedPawn.Row() - nextPosition));
            }
            foreach (Point position in positions)
            {
                if (IsInField(position) && !ShouldOccupy(position, redPawns, bluePawns))
                {
                    Point middlePoint = new Point(AverageCalculator.CalculateAverage(position.X, selectedPawn.Column()), AverageCalculator.CalculateAverage(position.Y, selectedPawn.Row()));
                    if (Occupy(middlePoint, oponentPlayer))
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        private static bool IsInField(Point position)
        {
            return position.X >= RuleConfig.firstColumn && position.X <= RuleConfig.lastColumn && position.Y >= RuleConfig.firstRow && position.Y <= RuleConfig.lastRow;
        }

        private bool ShouldOccupy(Point position, List<PawnAdapter> redPawns, List<PawnAdapter> bluePawns)
        {
            return Occupy(position, redPawns) || Occupy(position, bluePawns);
        }

        private bool Occupy(Point point, List<PawnAdapter> bandits)
        {
            foreach (PawnAdapter bandit in bandits)
            {
                if (point.X == bandit.Column() && point.Y == bandit.Row())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
