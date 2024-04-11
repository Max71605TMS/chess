using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Bishop : Figure
    {
        public Bishop(bool isWhite, Point point) : base(isWhite, point)
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
                if(isChoosen)
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
                if (isChoosen)
                {
                    return Properties.Resources.Bishop_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Bishop_Black_White : Properties.Resources.Castle_Black_Black;
                }
                

            }
        }
    }
}
