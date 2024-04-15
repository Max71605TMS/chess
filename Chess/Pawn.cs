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
        private bool _isDirectionUp;

        private const int BoardSize = 8;

        public Pawn(bool isWhite, Point point, bool isDirectionUp) : base(isWhite, point)
        {
            _isDirectionUp = isDirectionUp;
        }

        public bool IsFirstTurn { get; set; } = true;

        private IEnumerable<Point> GetAttackPositions(IEnumerable<Figure> figures, int offsetX1, int offsetY1, int offsetX2, int offsetY2)
        {
            var positions = figures.Where(f => f.Position == new Point(Position.X + offsetX1, Position.Y + offsetY1)
                                            || f.Position == new Point(Position.X + offsetX2, Position.Y + offsetY2)).Select(f => f.Position);

            return positions;
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();
            int direction = _isDirectionUp && IsWhite ? -1 : 1;

            if ((Position.Y + direction) >= 1 && (Position.Y + direction) <= BoardSize)
            {
                if (!figures.Any(f => f.Position == new Point(Position.X, Position.Y + direction)))
                {
                    positions.Add(new Point(Position.X, Position.Y + direction));

                    positions.AddRange(GetAttackPositions(figures, 1, direction, -1, direction));

                    if (IsFirstTurn && !figures.Any(f => f.Position == new Point(Position.X, Position.Y + direction)))
                    {
                        positions.Add(new Point(Position.X, Position.Y + direction * 2));
                    }
                }
            }

            return positions;
        }

        public override Image GetImage()
        {
            if (IsWhite)
            {
                if (IsChoosen)
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
                if (IsChoosen)
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
