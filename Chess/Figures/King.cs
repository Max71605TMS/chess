using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class King : Figure, IFigureRestriction, ICastling
{
    public King(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var kingMovePositions = GetMovePositions(figures);
        var availablePositions = CheckAvailablePositions(figures, kingMovePositions);

        if (!IsFirstTurn || !IsSelected) return availablePositions;

        var castlingPositions = new List<Point>();

        castlingPositions.AddRange(((ICastling)this).GetCastlingFiguresByDirection(figures, this, -1));
        castlingPositions.AddRange(((ICastling)this).GetCastlingFiguresByDirection(figures, this, 1));

        if (castlingPositions.Count > 0)
            availablePositions.AddRange(((ICastling)this).CheckCastlingPositions(figures, this, castlingPositions));

        return availablePositions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.KingWhite);
            return (Position.X + Position.Y) % 2 == 0
                ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.KingWhite)
                : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.KingWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.KingBlack);
        return (Position.X + Position.Y) % 2 == 0
            ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.KingBlack)
            : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.KingBlack);
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

            if (figure is null)
            {
                positions.Add(point);

                continue;
            }

            if (figure.IsWhite != IsWhite)
                positions.Add(point);
        }

        return positions;
    }

    private List<Point> CheckAvailablePositions(IEnumerable<Figure> figures,
        IEnumerable<Point> kingPositions)
    {
        var possibleMoves = new List<Point>();

        possibleMoves.AddRange(kingPositions);

        if (!IsSelected) return possibleMoves;

        var t = figures.Where(figure => figure.IsWhite != IsWhite);
        var w = figures.FirstOrDefault(f => f is King king && king.IsWhite == IsWhite);
        var r = t.Select(figure =>
            figure.GetAvailablePositions(
                figures.Except([w])));
        var e = r.Aggregate(possibleMoves,
            (current, positions) => current.Except(positions).ToList());

        possibleMoves = figures.Where(figure => figure.IsWhite != IsWhite)
            .Select(figure =>
                figure.GetAvailablePositions(figures.Except([
                    figures.FirstOrDefault(f => f is King king && king.IsWhite == IsWhite)
                ]))).Aggregate(possibleMoves,
                (current, positions) => current.Except(positions).ToList());

        if (possibleMoves.Count == 1 &&
            figures.FirstOrDefault(f => f.Position == possibleMoves.First()) is { } figure &&
            figure.IsWhite != IsWhite && figures.Where(w => w.IsWhite != IsWhite)
                .Select(s => s.GetAvailablePositions(figures.Except([figure]))).SelectMany(s => s)
                .Any(a => a == possibleMoves.First()))
            possibleMoves.Clear();

        return possibleMoves;
    }
}