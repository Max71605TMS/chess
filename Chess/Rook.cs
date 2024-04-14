using Chess.Interfaces;
using Chess.Properties;

namespace Chess;

public class Rook : Figure, IMarkable
{
    public bool IsFirstTurn { get; set; } = true;

    public Rook(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        positions.AddRange(GetPositionsByDirection(figures, -1, 0)); // По горизонтали слева
        positions.AddRange(GetPositionsByDirection(figures, 1, 0));  // По горизонтали справа
        positions.AddRange(GetPositionsByDirection(figures, 0, -1)); // По вертикали сверху
        positions.AddRange(GetPositionsByDirection(figures, 0, 1));  // По вертикали снизу

        return positions;
    }

    private IEnumerable<Point> GetPositionsByDirection(IEnumerable<Figure> figures, int xDirection, int yDirection)
    {
        var positions = new List<Point>();
        var x = Position.X + xDirection;
        var y = Position.Y + yDirection;

        while (x is >= 0 and <= 7 && y is >= 0 and <= 7)
        {
            var figure = figures.FirstOrDefault(f => f.Position == new Point(x, y));

            if (figure is not null)
            {
                if (figure.IsWhite != IsWhite)
                    positions.Add(new Point(x, y));
                break;
            }

            positions.Add(new Point(x, y));

            x += xDirection;
            y += yDirection;
        }

        return positions;
    }

    public override Image GetImage()
    {
        if (IsWhite)
        {
            if (IsChoosen)
                return Resources.Castle_White_Green;
            
            return (Position.X + Position.Y) % 2 == 0 ? Resources.Castle_White_White : Resources.Castle_White_Black;
        }

        if (IsChoosen)
            return Resources.Castle_Black_Green;
        return (Position.X + Position.Y) % 2 == 0 ? Resources.Castle_Black_White : Resources.Castle_Black_Black;
    }
}