using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Queen : Figure, IMoveByDirection
{
    public Queen(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, 1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, 1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, -1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, -1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, 0));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, 0));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, -1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, 1));

        return positions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.QueenWhite);
            return (Position.X + Position.Y) % 2 == 0
                ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.QueenWhite)
                : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.QueenWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.QueenBlack);
        return (Position.X + Position.Y) % 2 == 0
            ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.QueenBlack)
            : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.QueenBlack);
    }
}