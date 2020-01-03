using Checkers.Adapters;
using Checkers.Entities;
using Checkers.Enums;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.Factories
{
    class PawnAdapterFactory
    {
        public PawnAdapter Create(PlayerEnum playerEnum, PictureBox pictureBox)
        {
            PawnBox pawnBox = new PawnBox(playerEnum, PawnTypeEnum.Normal, 0, 0, Color.Black, null, true);
            return new PawnAdapter(pictureBox, pawnBox);
        }
    }
}
