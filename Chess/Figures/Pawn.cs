using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Pawn : Figure, IFigureRestriction
{
    private const int BoardSize = 8;

    public Pawn(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();
        if (IsWhite)
        {
            var isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y - 1));
            if (Position.Y > 0 && !isAnyFiguresBehind) positions.Add(new Point(Position.X, Position.Y - 1));
            var isAnyFiguresBehindPlus = figures.Any(f => f.Position == new Point(Position.X, Position.Y - 2));
            if (IsFirstTurn && !isAnyFiguresBehind && !isAnyFiguresBehindPlus)
                positions.Add(new Point(Position.X, Position.Y - 2));

            var isAnyEnemyFigureDiagonallyToTheLeft = figures.Any(f =>
                f.IsWhite == !IsWhite && f.Position == new Point(Position.X - 1, Position.Y - 1));
            if (isAnyEnemyFigureDiagonallyToTheLeft) positions.Add(new Point(Position.X - 1, Position.Y - 1));

            var isAnyEnemyFigureDiagonallyToTheRight = figures.Any(f =>
                f.IsWhite == !IsWhite && f.Position == new Point(Position.X + 1, Position.Y - 1));
            if (isAnyEnemyFigureDiagonallyToTheRight) positions.Add(new Point(Position.X + 1, Position.Y - 1));
        }
        else
        {
            var isAnyFiguresBehind = figures.Any(f => f.Position == new Point(Position.X, Position.Y + 1));
            if (Position.Y < BoardSize && !isAnyFiguresBehind) positions.Add(new Point(Position.X, Position.Y + 1));

            var isAnyFiguresBehindPlus = figures.Any(f => f.Position == new Point(Position.X, Position.Y + 2));
            if (IsFirstTurn && !isAnyFiguresBehind && !isAnyFiguresBehindPlus)
                positions.Add(new Point(Position.X, Position.Y + 2));

            var isAnyEnemyFigureDiagonallyToTheLeft = figures.Any(f =>
                f.IsWhite == !IsWhite && f.Position == new Point(Position.X - 1, Position.Y + 1));
            if (isAnyEnemyFigureDiagonallyToTheLeft) positions.Add(new Point(Position.X - 1, Position.Y + 1));

            var isAnyEnemyFigureDiagonallyToTheRight = figures.Any(f =>
                f.IsWhite == !IsWhite && f.Position == new Point(Position.X + 1, Position.Y + 1));
            if (isAnyEnemyFigureDiagonallyToTheRight) positions.Add(new Point(Position.X + 1, Position.Y + 1));
        }


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
}