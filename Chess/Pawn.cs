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
                if (Position.Y > 0 && !isAnyFiguresBehind)
                {
                    positions.Add(new Point(Position.X, Position.Y - 1));
                }
                var isAnyFiguresBehindPlus = figures.Any(f => f.Position == new Point(Position.X, Position.Y - 2));
                if (IsFirstTurn && !isAnyFiguresBehind && !isAnyFiguresBehindPlus)
                {
                    positions.Add(new Point(Position.X, Position.Y - 2));
                }

                var isAnyEnemyFigureDiagonallyToTheLeft = figures.Any(f => f.isWhite == !this.isWhite && f.Position == new Point(Position.X - 1, Position.Y - 1));
                if (isAnyEnemyFigureDiagonallyToTheLeft)
                {
                    positions.Add(new Point(Position.X - 1, Position.Y - 1));
                }

                var isAnyEnemyFigureDiagonallyToTheRight = figures.Any(f => f.isWhite == !this.isWhite && f.Position == new Point(Position.X + 1, Position.Y - 1));
                if (isAnyEnemyFigureDiagonallyToTheRight)
                {
                    positions.Add(new Point(Position.X + 1, Position.Y - 1));
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

                var isAnyEnemyFigureDiagonallyToTheLeft = figures.Any(f => f.isWhite == !this.isWhite && f.Position == new Point(Position.X - 1, Position.Y + 1));
                if (isAnyEnemyFigureDiagonallyToTheLeft)
                {
                    positions.Add(new Point(Position.X - 1, Position.Y + 1));
                }

                var isAnyEnemyFigureDiagonallyToTheRight = figures.Any(f => f.isWhite == !this.isWhite && f.Position == new Point(Position.X + 1, Position.Y + 1));
                if (isAnyEnemyFigureDiagonallyToTheRight)
                {
                    positions.Add(new Point(Position.X + 1, Position.Y + 1));
                }
            }
            
            //add all positions




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
