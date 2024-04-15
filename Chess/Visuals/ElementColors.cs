using Color = System.Drawing.Color;

namespace Chess.Visuals;

public static class ElementColors
{
    public static Color GetElementColor(ElementColor color, Point point = default)
    {
        var x = point.X;
        var y = point.Y;

        return color switch
        {
            ElementColor.White => Color.FromArgb(200, 200, 200),
            ElementColor.Black => Color.FromArgb(80, 80, 80),
            ElementColor.Green => (x + y) % 2 == 0 ? Color.FromArgb(0, 120, 0) : Color.FromArgb(0, 80, 0),
            ElementColor.Red => (x + y) % 2 == 0 ? Color.FromArgb(120, 0, 0) : Color.FromArgb(80, 0, 0),
            ElementColor.Castling => (x + y) % 2 == 0 ? Color.FromArgb(50, 81, 173) : Color.FromArgb(33, 54, 115),
            _ => Color.Pink
        };
    }
}

public enum ElementColor
{
    White,
    Black,
    Green,
    Red,
    Castling
}