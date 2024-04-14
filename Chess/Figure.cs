using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Figure
    {
        public Point Position { get; set; }

        public Image Image { get; set; }

        public bool IsChoosen { get; set; }

        public bool IsWhite {  get; set; }

        public Figure(bool isWhite, Point point)
        {
            IsWhite = isWhite;
            Position = point;
        }

        public abstract IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures);

        public abstract Image GetImage();

        public  IEnumerable<Point> WalkStraight(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();

            positions.AddRange(GetPositionsByDirection(figures, -1, 0)); // По горизонтали слева
            positions.AddRange(GetPositionsByDirection(figures, 1, 0));  // По горизонтали справа
            positions.AddRange(GetPositionsByDirection(figures, 0, -1)); // По вертикали сверху
            positions.AddRange(GetPositionsByDirection(figures, 0, 1));  // По вертикали снизу

            return positions;
        }

        private  IEnumerable<Point> GetPositionsByDirection(IEnumerable<Figure> figures, int xDirection, int yDirection)
        {
            var positions = new List<Point>();
            var x =Position.X + xDirection;
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
