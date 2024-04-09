using Chess.Properties;
using System.Drawing.Printing;
using System.Resources;

namespace Chess
{
    public partial class Form1 : Form
    {
        private const int BoardSize = 8;
        private const int ButtonSize = 75;
       
        private readonly Button[,] chessButtons = new Button[BoardSize, BoardSize];

        public Form1()
        {
            InitializeComponent();
            InitializeChessBoard();
        }

        
        private void InitializeChessBoard()
        { 
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    chessButtons[x, y] = Initializer.GetButton(ButtonSize, x, y, 150, 50);
                    this.Controls.Add(chessButtons[x, y]);
                    chessButtons[x, y].Click += ChessButton_Click;
                }
            }
        }

        private void ChessButton_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        
    }
}
