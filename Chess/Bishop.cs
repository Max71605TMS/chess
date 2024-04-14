using Chess.Interfaces;
using Chess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Bishop : Figure, IMarkable
    {
        private const int BoardSize = 8;

        public bool IsFirstTurn { get; set; } = true;

        public Bishop(bool isWhite, Point point) : base(isWhite, point)
        {
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();

            positions.AddRange(GetPositionsByDirection(figures, -1, -1)); // По горизонтали слева
            positions.AddRange(GetPositionsByDirection(figures, 1, 1));  // По горизонтали справа
            positions.AddRange(GetPositionsByDirection(figures, 1, -1)); // По вертикали сверху
            positions.AddRange(GetPositionsByDirection(figures, -1, 1));  // По вертикали снизу

            return positions;
        }

        public override Image GetImage()
        {
            if (IsWhite)
            {
                if (IsChoosen)
                {
                    return Properties.Resources.Bishop_White_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Bishop_White_White : Properties.Resources.Bishop_White_Black;
                }

            }
            else
            {
                if (IsChoosen)
                {
                    return Properties.Resources.Bishop_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Bishop_Black_White : Properties.Resources.Bishop_Black_Black;
                }
            }
        }

        private IEnumerable<Point> GetPositionsByDirection(IEnumerable<Figure> figures, int xDirection, int yDirection)
        {
            var positions = new List<Point>();
            var x = Position.X + xDirection;
            var y = Position.Y + yDirection;

            while (x is >= 0 and <= 7 && y is >= 0 and <= 7)
            {
                var figure = figures.FirstOrDefault(f => f.Position == new Point(x, y));

                if (figure is not null)
                {
                    if (figure.IsWhite != IsWhite)
                        positions.Add(new Point(x, y));
                    break;
                }

                positions.Add(new Point(x, y));

                x += xDirection;
                y += yDirection;
            }

            return positions;
        }
    }
}
