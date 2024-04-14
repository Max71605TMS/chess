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

        private bool IsPositionOccupied(IEnumerable<Figure> figures, Point position)
        {
            return figures.Any(f => f.Position == position);
        }

        private void AddPositionIfFree(List<Point> positions, IEnumerable<Figure> figures, Point position)
        {
            if (!IsPositionOccupied(figures, position))
            {
                positions.Add(position);
            }
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();

            var forwardStep = _isDirectionUp ? -1 : 1;
            var doubleForwardStep = _isDirectionUp ? -2 : 2;

            var forwardPosition = new Point(Position.X, Position.Y + forwardStep);
            var doubleForwardPosition = new Point(Position.X, Position.Y + doubleForwardStep);

            var isAnyFiguresBehind = IsPositionOccupied(figures, forwardPosition);
            var attackingPositions = figures.Where(f =>
                f.Position == new Point(Position.X - 1, Position.Y + forwardStep) ||
                f.Position == new Point(Position.X + 1, Position.Y + forwardStep)).ToList();

            if (Position.Y != 1 && Position.Y != BoardSize)
            {
                AddPositionIfFree(positions, figures, forwardPosition);

                foreach (var attackingPosition in attackingPositions)
                {
                    positions.Add(attackingPosition.Position);
                }

                if (IsFirstTurn && !isAnyFiguresBehind && !IsPositionOccupied(figures, doubleForwardPosition))
                {
                    positions.Add(doubleForwardPosition);
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
