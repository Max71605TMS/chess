﻿using Chess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Queen : Figure
    {
        public Queen(bool isWhite, Point point) : base(isWhite, point)
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
                    return Resources.Queen_White_Green;

                return (Position.X + Position.Y) % 2 == 0 ? Resources.Queen_White_White : Resources.Queen_White_Black;
            }

            if (isChoosen)
                return Resources.Queen_Black_Green;
            return (Position.X + Position.Y) % 2 == 0 ? Resources.Queen_Black_White : Resources.Queen_Black_Black;
        }
    }
}
