using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class FigureMover
    {
        private const int BoardSize = 8;

        public bool IsWhiteTurn { get; private set; } = true;

        public IEnumerable<Point> AvaliablePositions { get; private set; }

        public Figure Currentfigure { get; private set; }

        public List<Figure> Figures { get; private set; }

        public FigureMover(bool isWhiteDown) => Figures = Initializer.GetFigures(isWhiteDown);

        public void ChooseFigure(Figure figure)
        {
            if (figure.isWhite == IsWhiteTurn)
            {
                AvaliablePositions = figure.GetAvaliablePositions(Figures);
                figure.isChoosen = true;
                Currentfigure = figure;
            }
        }

        public Figure GetFigure(Point point) => Figures.FirstOrDefault(x => x.Position == point);

        public void Move(Point point)
        {
            Currentfigure.isChoosen = false;
            if (AvaliablePositions.Any(position => position == point))
            {

                if (Currentfigure is IMarkable markableFigure)
                {
                    markableFigure.IsFirstTurn = false;
                }

                Currentfigure.Position = point;
                IsWhiteTurn = !IsWhiteTurn;

                var attactedFigure = Figures.FirstOrDefault(figure => figure.Position == point && figure.isWhite != Currentfigure.isWhite);

                if (attactedFigure != null)
                {
                    Figures.Remove(attactedFigure);
                }
            }
        }
    }
}
