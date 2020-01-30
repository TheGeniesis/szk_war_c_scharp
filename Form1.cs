using Checkers.Adapters;
using Checkers.Enums;
using Checkers.Factories;
using Checkers.Services;
using Checkers.Transformators;
using Checkers.Validators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
    public partial class Form1 : Form
    {
        private int turn = 0;
        private bool extraMove = false;
        private PawnAdapter selectedPawn = null;
        private int leftBlue;
        private int leftRed;

        readonly private List<PawnAdapter> bluePawns = new List<PawnAdapter>();
        readonly private List<PawnAdapter> redPawns = new List<PawnAdapter>();
        readonly private PawnAdapterFactory PawnAdapterFactory;
        readonly private BasicMoveValidator BasicMoveValidator;
        readonly private JumpMoveValidator JumpMoveValidator;
        readonly private ExtraMoveValidator ExtraMoveValidator;
        readonly private BeatPawnService BeatPawnService;
        readonly private RowToUserReadableTransformator RowToUserReadableTransformator;
        public Form1()
        {
            PawnAdapterFactory = new PawnAdapterFactory();
            BasicMoveValidator = new BasicMoveValidator();
            JumpMoveValidator = new JumpMoveValidator();
            ExtraMoveValidator = new ExtraMoveValidator();
            BeatPawnService = new BeatPawnService();
            RowToUserReadableTransformator = new RowToUserReadableTransformator();

            InitializeComponent();
            LoadList();
        }
        private void LoadList()
        {
            List<PictureBox> pawns = new List<PictureBox>
            {
                bluePawn1,
                bluePawn2,
                bluePawn3,
                bluePawn4,
                bluePawn5,
                bluePawn6,
                bluePawn7,
                bluePawn8,
                bluePawn9,
                bluePawn10,
                bluePawn11,
                bluePawn12
            };

            foreach (PictureBox pawn in pawns)
            {
                bluePawns.Add(PawnAdapterFactory.Create(PlayerEnum.Blue, pawn));
            }
            leftBlue = pawns.Count;

            pawns = new List<PictureBox>
            {
                redPawn1,
                redPawn2,
                redPawn3,
                redPawn4,
                redPawn5,
                redPawn6,
                redPawn7,
                redPawn8,
                redPawn9,
                redPawn10,
                redPawn11,
                redPawn12
            };

            foreach (PictureBox pawn in pawns)
            {
                redPawns.Add(PawnAdapterFactory.Create(PlayerEnum.Red, pawn));
            }
            leftRed = pawns.Count;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void MoveOn(PawnAdapter pawn)
        {

            if (null == selectedPawn)
            {
                return;
            }

            if (!BasicMoveValidator.IsValid(selectedPawn, pawn) && !JumpMoveValidator.IsValid(selectedPawn, pawn, bluePawns, redPawns))
            {
                return;
            }

            bool beaten = BeatPawnService.TryToBeatPawn(selectedPawn, pawn, bluePawns, redPawns);

            int oldRow = selectedPawn.Row();
            int oldColumn = selectedPawn.Column();
            selectedPawn.Row(pawn.Row());
            selectedPawn.Column(pawn.Column());
            selectedPawn.CheckAndConvertToJumper();

            ShowLogMessage(pawn, oldRow, oldColumn);
            UpdateResult(beaten);

            if (beaten && ExtraMoveValidator.IsValid(selectedPawn, bluePawns, redPawns))
            {
                extraMove = true;
                return;
            }
            extraMove = false;
            selectedPawn.Color(Color.Black);
            selectedPawn = null;
            ++turn;
        }

        private void ShowLogMessage(PawnAdapter pawn, int oldRow, int oldColumn)
        {
            statusText.Text = string.Format("Player: {0} moved from: {1}{2} to: {3}{4} \n{5}",
                selectedPawn.Player().ToString(),
                RowToUserReadableTransformator.Transform(oldColumn),
                oldRow.ToString(),
                RowToUserReadableTransformator.Transform(pawn.Column()),
                pawn.Row().ToString(),
                statusText.Text
            );
        }

        private void UpdateResult(bool beaten)
        {
            if (!beaten)
            {
                return; 
            }

            if (this.selectedPawn.Player() == PlayerEnum.Red)
            {
                leftBlue--;
                if (0 == leftBlue)
                {
                    MessageBox.Show(this.selectedPawn.Player() + " player won");
                }
                    
                return;
            }

            leftRed--;
            if (0 == leftRed)
            {
                MessageBox.Show(this.selectedPawn.Player() + "player won");
            }
        }

 
        public void Select(Object obj)
        {
            if (extraMove)
            {
                return;
            }

            if (this.selectedPawn != null)
            {
                this.selectedPawn.Color(Color.Black);
            }

            ChoosePawn((PictureBox)obj);

            if (this.selectedPawn == null)
            {
                throw (new ArgumentOutOfRangeException("can't select field"));
            }
            this.selectedPawn.Color(Color.Lime);
        }

        private void ChoosePawn(PictureBox file)
        {
            PawnAdapter tempPawn = PawnAdapterFactory.Create(PlayerEnumMethods.GetPlayer(file.Name.ToString()), file);
            if (PlayerEnumMethods.GetPlayer(file.Name.ToString()) == PlayerEnum.Red)
            {
                foreach (PawnAdapter pawn in redPawns)
                {
                    if (tempPawn.Equals(pawn))
                    {
                        this.selectedPawn = pawn;
                    }
                }
            }
            if (PlayerEnumMethods.GetPlayer(file.Name.ToString()) == PlayerEnum.Blue)
            {
                foreach (PawnAdapter pawn in bluePawns)
                {
                    if (tempPawn.Equals(pawn))
                    {
                        this.selectedPawn = pawn;
                    }
                }
            }
        }

        private void ImageClick(Object sender, MouseEventArgs e)
        {
            PictureBox file = (PictureBox)sender;
            MoveOn(PawnAdapterFactory.Create(PlayerEnumMethods.GetPlayer(file.Name.ToString()), file));
        }

        private void SelectBluePawn(Object sender, MouseEventArgs e)
        {
            if (turn % 2 == 1)
            {
                Select(sender);
                return;
            }
            MessageBox.Show("It's red turn");
        }

        private void SelectRedPawn(Object sender, MouseEventArgs e)
        {
            if (turn % 2 == 0)
            {
                Select(sender);
                return;
            }
            MessageBox.Show("It's blue turn");
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox30_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox38_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox44_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox47_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox49_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox50_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox51_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox52_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox53_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox54_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox55_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox56_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox57_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox58_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox59_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox60_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox61_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox62_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox63_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox64_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn12_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn11_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn10_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn9_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn8_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn7_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn6_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn5_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn4_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn3_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn2_Click(object sender, EventArgs e)
        {

        }

        private void bluePawn1_Click(object sender, EventArgs e)
        {

        }

        private void redPawn12_Click(object sender, EventArgs e)
        {

        }

        private void redPawn11_Click(object sender, EventArgs e)
        {

        }

        private void redPawn10_Click(object sender, EventArgs e)
        {

        }

        private void redPawn9_Click(object sender, EventArgs e)
        {

        }

        private void redPawn8_Click(object sender, EventArgs e)
        {

        }

        private void redPawn7_Click(object sender, EventArgs e)
        {

        }

        private void redPawn6_Click(object sender, EventArgs e)
        {

        }

        private void redPawn5_Click(object sender, EventArgs e)
        {

        }

        private void redPawn4_Click(object sender, EventArgs e)
        {

        }

        private void redPawn3_Click(object sender, EventArgs e)
        {

        }

        private void redPawn2_Click(object sender, EventArgs e)
        {

        }

        private void redPawn1_Click(object sender, EventArgs e)
        {

        }

        private void logLabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }
}
