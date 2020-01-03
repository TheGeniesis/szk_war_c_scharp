using Checkers.Configs;
using Checkers.Entities;
using Checkers.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.Adapters
{
    class PawnAdapter
    {
        private readonly int size = 50;
        private readonly PictureBox pictureBox;
        private readonly PawnBox pawnBox;

        public PawnAdapter(PictureBox pictureBox, PawnBox pawnBox)
        {
            this.pictureBox = pictureBox;
            this.pawnBox = pawnBox;

            this.pawnBox.Column = pictureBox.Location.X / this.size;
            this.pawnBox.Row = pictureBox.Location.Y / this.size;

            this.pawnBox.BackgroundImage = pictureBox.BackgroundImage;
        }
        public string Name()
        {
            return this.pictureBox.Name.ToString();
        }

        public int Row() 
        {
            return this.pawnBox.Row;
        }
        public void Row(int row) 
        {
            if (row > 8)
            {
                throw (new IndexOutOfRangeException("Out of range"));
            }
            
            if (row < 0)
            {
                throw (new IndexOutOfRangeException("Out of range"));
            }

            this.pawnBox.Row = row;
            Update();
        }
        public int Column() 
        {
            return this.pawnBox.Column;
        }
        public void Column(int column)
        {
            if (column > 8)
            {
                throw (new IndexOutOfRangeException("Out of range"));
            }

            if (column < 0)
            {
                throw (new IndexOutOfRangeException("Out of range"));
            }

            this.pawnBox.Column = column;
            Update();
        }
        public Color Color() 
        {
            return this.pawnBox.Color;
        }
        public void Color(Color color)
        {
            this.pawnBox.Color = color;
            Update();
        }
        
        public PawnTypeEnum Type() 
        {
            return this.pawnBox.Type;
        }
        public void Type(PawnTypeEnum  type) 
        {
            if (this.pawnBox.Type == PawnTypeEnum.Jumper && type != PawnTypeEnum.Jumper)
            {
                throw (new InvalidCastException("Can modify only normal type"));
            } 
            this.pawnBox.Type = type;
        }
        
        public PlayerEnum Player() 
        {
            return this.pawnBox.Player;
        }

        public void CheckAndConvertToJumper()
        {
            if (this.pawnBox.Type == PawnTypeEnum.Jumper) {
                return;
            }

            if (PlayerEnum.Blue == this.pawnBox.Player && RuleConfig.lastRow == this.pawnBox.Row)
            {
                this.pawnBox.BackgroundImage = Properties.Resources.game_pawn_jumper_blue;
                this.pawnBox.Type = PawnTypeEnum.Jumper;
                Update();
            }
            else if (PlayerEnum.Red == this.pawnBox.Player && RuleConfig.firstRow == this.pawnBox.Row)
            {
                this.pawnBox.BackgroundImage = Properties.Resources.game_pawn_jumper_red;
                this.pawnBox.Type = PawnTypeEnum.Jumper;
                Update();
            }
        }

        public void Hide()
        {
            this.pawnBox.Row = 0;
            this.pawnBox.Column = 0;
            this.pawnBox.Visible = false;
            Update();
        }

        public bool Equals(PawnAdapter toCompare)
        {
            return toCompare.Row() == this.pawnBox.Row && toCompare.Column() == this.pawnBox.Column;
        }
        private void Update()
        {
            this.pictureBox.BackColor = this.pawnBox.Color;
            this.pictureBox.Location = new Point(this.pawnBox.Column * size, this.pawnBox.Row * size);
            this.pictureBox.BackgroundImage = this.pawnBox.BackgroundImage;
            this.pictureBox.Visible = this.pawnBox.Visible;
        }
    }
}
