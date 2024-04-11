using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chess
{
    internal static class Initializer
    {
        internal static Button GetButton(int ButtonSize, int x, int y, int offestX = 0, int offsetY = 0)
        {
            var figure = GetFigure(x, y, true);
            return new Button
            {
                Width = ButtonSize,
                Height = ButtonSize,
                Location = new Point(x * ButtonSize + offestX, y * ButtonSize + offsetY),
                BackColor = (x + y) % 2 == 0 ? Color.White : Color.Black,
                Tag = figure != null ? figure : new Point(x,y),
                Image = figure != null ? figure.GetImage() : null
            };
        }

        private static Figure GetFigure(int x, int y, bool isWhiteDown)
        {
            if(y == 0)
            {
                if(x == 4)
                {
                    return new King(false, new Point(x,y));
                }
            } else if(y == 1)
            {
                return new Pawn(false, new Point(x, y), isWhiteDown);
            }
            else if (y == 6)
            {
                return new Pawn(true, new Point(x, y), !isWhiteDown);
            }
            else if (y == 7)
            {
                if (x == 4)
                {
                    return new King(true, new Point(x, y));
                }
            } else if (y == 7)
            {
                if(x == 2 || x == 6)
                {
                    return new Horse(true, new Point(x, y));
                }
            }
            else if (y == 0)
            {
                if(x == 2 || x == 6)
                {
                    return new Horse(false, new Point(x, y));
                }
            }

            return null;
        }

        
    }
}
