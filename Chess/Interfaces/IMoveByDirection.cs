using Chess.Abstract;

namespace Chess.Interfaces;

public interface IMoveByDirection
{
    public IEnumerable<Point> GetPositionsByDirection(IEnumerable<Figure> figures, Figure figure, int xDirection,
        int yDirection)
    {
        var positions = new List<Point>();
        var x = figure.Position.X + xDirection;
        var y = figure.Position.Y + yDirection;

        while (x is >= 0 and <= 7 && y is >= 0 and <= 7)
        {
            var figureOnTheWay = figures.FirstOrDefault(f => f.Position == new Point(x, y));

            if (figureOnTheWay is not null)
            {
                if (figureOnTheWay.IsWhite != figure.IsWhite)
                    positions.Add(new Point(x, y));
                break;
            }

            positions.Add(new Point(x, y));

            x += xDirection;
            y += yDirection;
        }

        return positions;
    }
}