using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Pawn : Figure, IMarkable
    {
        private const int BoardSize = 8;
        
        private bool _isDirectionUp;
        public Pawn(bool isWhite, Point point, bool isDirectionUp) : base(isWhite, point)
        {
            _isDirectionUp = isDirectionUp;
        }

        public bool IsFirstTurn { get; set; } = true;

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
           
            var positions = new List<Point>();
            if (_isDirectionUp && isWhite)
            {
                var isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y - 1));
                if (Position.Y > 1 && !isAnyFiguresBehind)
                {
                    positions.Add(new Point(Position.X, Position.Y - 1));
                }
                var isAnyFiguresBehindPlus = figures.Any(f => f.Position == new Point(Position.X, Position.Y - 2));
                if (IsFirstTurn && !isAnyFiguresBehind && !isAnyFiguresBehindPlus)
                {
                    positions.Add(new Point(Position.X, Position.Y - 2));
                }
                
            } else
            {
                var isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y + 1));
                if (Position.Y < BoardSize && !isAnyFiguresBehind)
                {
                    positions.Add(new Point(Position.X, Position.Y + 1));
                }
                var isAnyFiguresBehindPlus = figures.Any(f => f.Position == new Point(Position.X, Position.Y + 2));
                if (IsFirstTurn && !isAnyFiguresBehind && !isAnyFiguresBehindPlus)
                {
                    positions.Add(new Point(Position.X, Position.Y + 2));
                }
            }

            //add all positions
            if (isWhite)
            {
                var blackFigures = figures.Where(f => f.isWhite == false).ToList();
                var pointAtackLeft = new Point(Position.X - 1, Position.Y - 1);
                var pointAtackRight = new Point(Position.X + 1, Position.Y - 1);
                
                
                    if (blackFigures.Any(f => f.Position == pointAtackLeft))
                    {
                         positions.Add(new Point(Position.X - 1, Position.Y - 1));
                    }

                    if (blackFigures.Any(f => f.Position == pointAtackRight))
                    {
                        positions.Add(new Point(Position.X + 1, Position.Y - 1));
                    }
            }
            else
            {
                var whiteFigures = figures.Where(f => f.isWhite).ToList();
                var pointAtackLeft = new Point(Position.X - 1, Position.Y + 1);
                var pointAtackRight = new Point(Position.X + 1, Position.Y + 1);

                if (whiteFigures.Any(f => f.Position == pointAtackLeft))
                {
                    positions.Add(pointAtackLeft);
                }

                if (whiteFigures.Any(f => f.Position == pointAtackRight))
                {
                    positions.Add(pointAtackRight);
                }
            }



            return positions;
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
                {
                    return Properties.Resources.Pawn_White_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Pawn_White_White : Properties.Resources.Pawn_White_Black;
                }

            }
            else
            {
                if (isChoosen)
                {
                    return Properties.Resources.Pawn_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Pawn_Black_White : Properties.Resources.Pawn_Black_Black;
                }
            }
        }
    }
}
