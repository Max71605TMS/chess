using Chess.Interfaces;
using Chess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Bishop : Figure, IMovementBishop
    {
        public Bishop(bool isWhite, Point point) : base(isWhite, point)
        {

        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var positions = new List<Point>();

            positions.AddRange(((IMovementBishop)this).GetMove(figures, this, -1, 1)); 
            positions.AddRange(((IMovementBishop)this).GetMove(figures, this, 1, 1));  
            positions.AddRange(((IMovementBishop)this).GetMove(figures, this, 1, -1)); 
            positions.AddRange(((IMovementBishop)this).GetMove(figures, this, -1, -1));  

            return positions;
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
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
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Bishop_Black_White : Properties.Resources.Bishop_Black_Black;
                }
            }
        }
    }
}
