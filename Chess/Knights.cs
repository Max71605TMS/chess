using Chess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Knights : Figure
    {
        public Knights(bool isWhite, Point point) : base(isWhite, point)
        {
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();
            for (var i = 0;i < 8; i++) 
            {
                for (var j = 0;j < 8;j++) 
                {
                    if (Math.Abs(i - Position.Y) * Math.Abs(j - Position.Y) == 2)
                    {
                        positions.Add(new Point(Position.X, Position.Y));
                    }
                }
            }
            return positions;
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
