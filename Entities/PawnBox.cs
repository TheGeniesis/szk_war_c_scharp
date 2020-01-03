using Checkers.Enums;
using System.Drawing;

namespace Checkers.Entities
{
    public class PawnBox
    {
        public PawnBox(PlayerEnum playerEnum, PawnTypeEnum pawnTypeEnum, int row, int column, Color color, Bitmap backgroundImage, bool visible) 
        {
            Player = playerEnum;
            Type = pawnTypeEnum;
            Row = row;
            Column = column;
            Color = color;
            BackgroundImage = backgroundImage;
            Visible = visible;
        }
        
        public PawnTypeEnum Type { get; set; }
        public int Column { get; set; }
        public Color Color { get; set; }
        public int Row { get; set; }
        public PlayerEnum Player { get; set; }
        public Image BackgroundImage { get; set; }
        public bool Visible { get; set; }
    }
}