using Chess.Properties;
using System.Drawing.Printing;
using System.Resources;

namespace Chess
{
    public partial class Form1 : Form
    {
        private const int BoardSize = 8;
        private const int ButtonSize = 75;

        private bool isWhiteTurn = true;

        private IEnumerable<Point> _avaliablePositions;

        private IEnumerable<Figure> _figures = new List<Figure>();

        private Figure _currentfigure;

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
            var button = (Button)sender;
            if(button.Tag is Figure figure)
            {
                if(figure.isWhite == isWhiteTurn)
                {
                    _avaliablePositions = figure.GetAvaliablePositions(_figures);
                    figure.isChoosen = true;
                    _currentfigure = figure;
                    chessButtons[figure.Position.X, figure.Position.Y].Image = figure.GetImage();
                    foreach(var position in _avaliablePositions)
                    {
                        chessButtons[position.X, position.Y].Image = Properties.Resources.Empty_Green; 
                        //TODO: add green for enemyes figures
                    }
                }
            } 
            else
            {
                var point = (Point)button.Tag;
                if(_avaliablePositions.Any(position => position == point))
                {
                    chessButtons[_currentfigure.Position.X, _currentfigure.Position.Y].Image = Properties.Resources.Empty_Black;
                    chessButtons[_currentfigure.Position.X, _currentfigure.Position.Y].Tag = new Point(point.X, point.Y);
                    chessButtons[point.X, point.Y].Tag = _currentfigure;
                    _currentfigure.isChoosen = false;
                    _currentfigure.Position = point;
                    chessButtons[point.X, point.Y].Image = _currentfigure.GetImage();
                    isWhiteTurn = !isWhiteTurn;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
