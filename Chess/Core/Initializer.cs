using Chess.Abstract;
using Chess.Figures;
using Chess.Visuals;

namespace Chess.Core;

internal static class Initializer
{
    private const int BoardSize = 8;

    internal static Button GetButton(int buttonSize, int x, int y, int offsetX = 0, int offsetY = 0)
    {
        return new Button
        {
            Width = buttonSize,
            Height = buttonSize,
            Location = new Point(x * buttonSize + offsetX, y * buttonSize + offsetY),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0
            },
            BackColor = (x + y) % 2 == 0
                ? ElementColors.GetElementColor(ElementColor.White, new Point(x, y))
                : ElementColors.GetElementColor(ElementColor.Black, new Point(x, y)),
            Tag = new Point(x, y),
            BackgroundImageLayout = ImageLayout.Stretch
        };
    }

    internal static Label GetLabel(Button button, string text)
    {
        return new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Pixel),
            ForeColor = SystemColors.Menu,
            Location = button.Location,
            Name = "label1",
            Size = new Size(26, 28),
            Text = text
        };
    }

    public static List<Figure> GetFigures()
    {
        var figures = new List<Figure>();
        for (var y = 0; y < BoardSize; y++)
        for (var x = 0; x < BoardSize; x++)
        {
            var figure = GetFigure(x, y);
            if (figure is not null) figures.Add(figure);
        }

        return figures;
    }


    private static Figure? GetFigure(int x, int y)
    {
        switch (y)
        {
            case 0 or 7:
                switch (x)
                {
                    case 0:
                    case 7:
                        return new Rook(y is 7, new Point(x, y));
                    case 1:
                    case 6:
                        return new Knight(y is 7, new Point(x, y));
                    case 2:
                    case 5:
                        return new Bishop(y is 7, new Point(x, y));
                    case 3:
                        return new Queen(y is 7, new Point(x, y));
                    case 4:
                        return new King(y is 7, new Point(x, y));
                }

                break;
            case 1 or 6:
                return new Pawn(y is 6, new Point(x, y));
        }

        return null;
    }
}