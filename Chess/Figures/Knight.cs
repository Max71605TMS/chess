using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Knight : Figure, IMoveByDirection
{
    public Knight(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();
        var possiblePositions = GetPossiblePositions();

        foreach (var position in possiblePositions)
        {
            var isFigureExist = figures.Any(f => f.Position == new Point(position.X, position.Y));

            if (isFigureExist)
            {
                var figure = figures.First(f => f.Position == new Point(position.X, position.Y));
                if (IsWhite != figure.IsWhite) positions.Add(figure.Position);
            }
            else
            {
                if (position.X is >= 0 and < 8 && position.Y is >= 0 and < 8)
                    positions.Add(new Point(position.X, position.Y));
            }
        }

        return positions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.KnightWhite);
            return (Position.X + Position.Y) % 2 == 0
                ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.KnightWhite)
                : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.KnightWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.KnightBlack);
        return (Position.X + Position.Y) % 2 == 0
            ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.KnightBlack)
            : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.KnightBlack);
    }

    private List<Point> GetPossiblePositions()
    {
        var positions = new List<Point>
        {
            new(Position.X - 1, Position.Y - 2),
            new(Position.X + 1, Position.Y - 2),
            new(Position.X - 1, Position.Y + 2),
            new(Position.X + 1, Position.Y + 2),
            new(Position.X - 2, Position.Y - 1),
            new(Position.X - 2, Position.Y + 1),
            new(Position.X + 2, Position.Y - 1),
            new(Position.X + 2, Position.Y + 1)
        };

        return positions;
    }
}