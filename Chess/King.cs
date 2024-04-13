using Chess.Interfaces;
using Chess.Properties;

namespace Chess;

public class King : Figure, IMarkable, ICastling
{
    public King(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
    {
        var kingMovePositions = GetMovePositions(figures);
        var availablePositions = CheckAvailablePositions(figures, kingMovePositions);

        if (!IsFirstTurn || !isChoosen) return availablePositions;

        availablePositions.AddRange(((ICastling)this).GetCastlingPositionsByDirection(figures, this, -1));
        availablePositions.AddRange(((ICastling)this).GetCastlingPositionsByDirection(figures, this, 1));

        return availablePositions;
    }

    private IEnumerable<Point> GetMovePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();
        var offsets = new List<Point>
        {
            new(1, 0),
            new(1, 1),
            new(0, 1),
            new(-1, 1),
            new(-1, 0),
            new(-1, -1),
            new(0, -1),
            new(1, -1)
        };

        foreach (var point in offsets.Select(offset =>
                     Position with { X = Position.X + offset.X, Y = Position.Y + offset.Y }))
        {
            if (point.X is < 0 or > 7 || point.Y is < 0 or > 7) continue;

            var figure = figures.FirstOrDefault(f => f.Position == point);

            if (figure is not null && figure.isWhite != isWhite)
                positions.Add(point);
            else
                positions.Add(point);
        }

        return positions;
    }

    private List<Point> CheckAvailablePositions(IEnumerable<Figure> figures,
        IEnumerable<Point> kingPositions)
    {
        var possibleMoves = new List<Point>();

        possibleMoves.AddRange(kingPositions);

        if (!isChoosen) return possibleMoves;

        return figures.Where(figure => figure.isWhite != isWhite)
            .Select(figure => figure.GetAvaliablePositions(figures)).Aggregate(possibleMoves,
                (current, positions) => current.Except(positions).ToList());
    }


    public override Image GetImage()
    {
        if (isWhite)
        {
            if (isChoosen)
                return Resources.King_White_Green;
            return (Position.X + Position.Y) % 2 == 0
                ? Resources.King_White_White
                : Resources.King_White_Black;
        }

        if (isChoosen)
            return Resources.King_Black_Green;
        return (Position.X + Position.Y) % 2 == 0
            ? Resources.King_Black_White
            : Resources.King_Black_Black;
    }
}