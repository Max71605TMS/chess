namespace Chess.Interfaces;

public interface ICastling
{
    public IEnumerable<Point> GetCastlingPositionsByDirection(IEnumerable<Figure> figures, Figure figure,
        int xDirection)
    {
        var positions = new List<Point>();
        var x = figure.Position.X + xDirection;
        var y = figure.Position.Y;

        while (x is >= 0 and <= 7 && y is >= 0 and <= 7)
        {
            var figureOnTheWay = figures.FirstOrDefault(f => f.Position == new Point(x, y));

            if (figureOnTheWay is not null)
            {
                if (figureOnTheWay.isWhite != figure.isWhite) break;

                if (figureOnTheWay is Rook { IsFirstTurn: true } or King { IsFirstTurn: true })
                    positions.Add(figureOnTheWay.Position);

                break;
            }

            x += xDirection;
        }

        return positions;
    }
}