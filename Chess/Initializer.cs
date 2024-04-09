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
            return new Button
            {
                Width = ButtonSize,
                Height = ButtonSize,
                Location = new Point(x * ButtonSize + offestX, y * ButtonSize+ offsetY),
                BackColor = (x + y) % 2 == 0 ? Color.White : Color.Black,
                Tag = null,
                Image = null
            };
        }

        
    }
}
