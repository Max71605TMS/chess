using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Pawn : Figure, IFigureRestriction
{
    public Pawn(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        if (IsSelected) positions.AddRange(GetMovementPositions(figures));

        positions.AddRange(GetAttackPositions(figures));

        return positions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.PawnWhite);
            return (Position.X + Position.Y) % 2 == 0
                       ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.PawnWhite)
                       : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.PawnWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.PawnBlack);
        return (Position.X + Position.Y) % 2 == 0
                   ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.PawnBlack)
                   : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.PawnBlack);
    }

    private IEnumerable<Point> GetMovementPositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        var moveCount = IsFirstTurn ? 2 : 1;

        for (var i = 1; i <= moveCount; i++)
        {
            var newPoint = Position with
            {
                X = Position.X,
                Y = IsWhite ? Position.Y - i : Position.Y + i
            };

            if (newPoint.X is < 0 or > 7 || newPoint.Y is < 0 or > 7) continue;

            var figure = figures.FirstOrDefault(f => f.Position == newPoint);

            if (figure is not null) continue;

            positions.Add(newPoint);
        }

        return positions;
    }

    private IEnumerable<Point> GetAttackPositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        for (var i = 0; i < 2; i++)
        {
            var newPoint = Position with
            {
                X = Position.X - 1 + 2 * i,
                Y = IsWhite ? Position.Y - 1 : Position.Y + 1
            };

            if (newPoint.X is < 0 or > 7 || newPoint.Y is < 0 or > 7) continue;

            var figure = figures.FirstOrDefault(f => f.Position == newPoint);

            if ((figure is null || figure.IsWhite == IsWhite) && IsSelected) continue;

            positions.Add(newPoint);
        }

        return positions;
    }
}