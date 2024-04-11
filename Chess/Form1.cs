using Chess.Properties;
using System.Drawing.Printing;
using System.Resources;

namespace Chess;

public partial class Form1 : Form
{
    private const int BoardSize = 8;
    private const int ButtonSize = 75;

    private readonly Button[,] chessButtons = new Button[BoardSize, BoardSize];

    private IEnumerable<Point> _avaliablePositions;

    private Figure _currentfigure;

    private readonly List<Figure> _figures = new();

    private readonly bool isWhiteTurn = true;

    public Form1()
    {
        InitializeComponent();
        InitializeChessBoard();
    }


    private void InitializeChessBoard()
    {
        for (var y = 0; y < BoardSize; y++)
        for (var x = 0; x < BoardSize; x++)
        {
            var button = Initializer.GetButton(ButtonSize, x, y, 150, 50);
            chessButtons[x, y] = button;
            Controls.Add(chessButtons[x, y]);
            chessButtons[x, y].Click += ChessButton_Click;

            if (button.Tag is Figure figure)
                _figures.Add(figure);
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
            } else
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
                }
            }

        }
    }
}