using Chess.Abstract;
using Chess.Figures;

namespace Chess.Interfaces;

public interface ICastling
{
    //Получить фигуры для рокировки. Проверка по направлению
    public IEnumerable<Point> GetCastlingFiguresByDirection(IEnumerable<Figure> figures, Figure figure,
                                                            int xDirection)
    {
        var positions = new List<Point>();
        var x = figure.Position.X + xDirection;
        var y = figure.Position.Y;

        while (x is >= 0 and <= 7)
        {
            var figureOnTheWay = figures.FirstOrDefault(f => f.Position == new Point(x, y));

            if (figureOnTheWay is not null &&
                (figureOnTheWay.IsWhite != figure.IsWhite || figureOnTheWay is not Rook and not King))
                break;

            if (figureOnTheWay is King)
            {
                positions.Add(new Point(x, y));
                break;
            }

            if (x is 0 or 7 && figureOnTheWay is not null && figureOnTheWay.IsWhite == figure.IsWhite &&
                ((IFigureRestriction)figureOnTheWay).IsFirstTurn)
                positions.Add(new Point(x, y));

            x += xDirection;
        }

        return positions;
    }

    //Проверка возможных путей рокировки на угрозу королю
    public IEnumerable<Point> CheckCastlingPositions(IEnumerable<Figure> figures, Figure figure,
                                                     IEnumerable<Point> castlingFiguresPositions)
    {
        var positions = new List<Point>();
        var king = figures.First(f => f is King { IsFirstTurn: true } king && king.IsWhite == figure.IsWhite);

        foreach (var position in castlingFiguresPositions.ToList())
        {
            int offset;

            if (position is { X: 4, Y: 7 } or { X: 4, Y: 0 })
                offset = figure.Position is { X: 0, Y: 7 } or { X: 0, Y: 0 } ? -2 : 2;
            else
                offset = position is { X: 0, Y: 7 } or { X: 0, Y: 0 } ? -2 : 2;

            var isUnderAttack = figures.Where(w => w.IsWhite != king.IsWhite)
                                       .Select(s => s.GetAvailablePositions(figures.Except([king]))).SelectMany(s => s)
                                       .Any(a => a == king.Position with { X = king.Position.X + offset });

            if (!isUnderAttack)
                positions.Add(position);
        }

        return positions;
    }
}