using System;

namespace Chess
{
    public class Horse : Figure

    {
        public Horse(bool isWhite, Point point) : base(isWhite, point)
        {

        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            throw new NotImplementedException();
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
                {
                    return Properties.Resources.Knight_Black_White;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Knight_Black_White : Properties.Resources.Knight_Black_Black;
                }

            }
            else
            {
                if (isChoosen)
                {
                    return Properties.Resources.Knight_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Knight_Black_White : Properties.Resources.Knight_Black_Black;
                }
            }
        }
    }
}

