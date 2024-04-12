using Chess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Knights : Figure
    {
        private const int BoardSize = 8;

        public Knights(bool isWhite, Point point) : base(isWhite, point)
        {
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    if ((Math.Abs(j - Position.X) * Math.Abs(i - Position.Y)) == 2)
                    {
                        positions.Add(new Point(j, i));
                    }
                }
            }

            var getAvaliablePositions = new List<Point>();

            if (isWhite)
            {
                var whiteFigures = figures.Where(f => f.isWhite).ToList();

                foreach (var move in positions)
                {
                    if (whiteFigures.All(f => f.Position != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }
            else
            {
                var blackFigures = figures.Where(f => f.isWhite == false).ToList();

                foreach (var move in positions)
                {
                    if (blackFigures.All(f => f.Position != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }

            return getAvaliablePositions;
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
                    return Resources.Knight_Black_Green;

                return (Position.X + Position.Y) % 2 == 0 ? Resources.Knight_Black_White : Resources.Knight_Black_Black;
            }

            if (isChoosen)
                return Resources.Knight_Black_Green;
            return (Position.X + Position.Y) % 2 == 0 ? Resources.Knight_Black_White : Resources.Knight_Black_Black;
        }
    }
}
