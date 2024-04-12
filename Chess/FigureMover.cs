using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class FigureMover
    {
        private const int BoardSize = 8;

        public bool _isWhiteTurn { get; private set; }  = true;

        public IEnumerable<Point> AvaliablePositions { get; private set; }

        public Figure _currentfigure { get; private set; }

        public List<Figure> Figures { get; private set; }
        
        public FigureMover(bool isWhiteDown)
        {
            Figures = Initializer.GetFigures(isWhiteDown);
        }

        public void ChooseFigure(Figure figure)
        {
            if (figure.isWhite == _isWhiteTurn)
            {
                AvaliablePositions = figure.GetAvaliablePositions(Figures);
                figure.isChoosen = true;
                _currentfigure = figure;
            }
        }

        public Figure GetFigure(Point point) 
        { 
            return Figures.FirstOrDefault(x=>x.Position == point);
        }

        public void Move(Point point)
        {
            
            _currentfigure.isChoosen = false;
            if (AvaliablePositions.Any(position => position == point))
            {

                if(_currentfigure is IMarkable markableFigure)
                {
                    markableFigure.IsFirstTurn = false;
                }

                _currentfigure.Position = point;
                _isWhiteTurn = !_isWhiteTurn;

                var attactedFigure = Figures.FirstOrDefault(figure => figure.Position == point && figure.isWhite != _currentfigure.isWhite);

                if (attactedFigure!= null)
                {
                    Figures.Remove(attactedFigure);
                }
            }
        }
    }
}
