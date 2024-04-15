using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Castle : Figure
    {
        private const int BoardSize = 8;

        public Castle(bool isWhite, Point point) : base(isWhite, point)
        {

        }

        public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();
            int step = 1;
            bool isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y + step));
            bool isAnyFiguresForward = figures.Any(f => f.Position == new Point(Position.X, Position.Y - step));
            bool isAnyFiguresLeft = figures.Any(f => f.Position == new Point(Position.X - step, Position.Y));
            bool isAnyFiguresRight = figures.Any(f => f.Position == new Point(Position.X + step, Position.Y));

            while (Position.Y + step < BoardSize && !isAnyFiguresBehind)
            {
                positions.Add(new Point(Position.X, Position.Y + step));
                step++;
                isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y + step));
            }
            step = 1;
            while (Position.Y - step > 0 && !isAnyFiguresBehind)
            {
                positions.Add(new Point(Position.X, Position.Y - step));
                step++;
                isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y - step));
            }
            step = 1;
            while (Position.X - step > 0 && !isAnyFiguresBehind)
            {
                positions.Add(new Point(Position.X - step, Position.Y));
                step++;
                isAnyFiguresLeft = figures.Any(f => f.Position == new Point(Position.X - step, Position.Y));
            }
            step = 1;
            while (Position.X + step < BoardSize && !isAnyFiguresBehind)
            {
                positions.Add(new Point(Position.X + step, Position.Y));
                step++;
                isAnyFiguresRight = figures.Any(f => f.Position == new Point(Position.X + step, Position.Y));
            }

            return positions;
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
                {
                    return Properties.Resources.Castle_White_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Castle_White_White : Properties.Resources.Castle_White_Black;
                }
            }
            else
            {
                if (isChoosen)
                {
                    return Properties.Resources.Castle_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Castle_Black_White : Properties.Resources.Castle_Black_Black;
                }
            }
        }
    }
}
