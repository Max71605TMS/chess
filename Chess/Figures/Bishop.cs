using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Bishop : Figure, IMoveByDirection
{
    public Bishop(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, 1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, 1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, -1));
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, -1));

        return positions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.BishopWhite);
            return (Position.X + Position.Y) % 2 == 0
                       ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.BishopWhite)
                       : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.BishopWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.BishopBlack);
        return (Position.X + Position.Y) % 2 == 0
                   ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.BishopBlack)
                   : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.BishopBlack);
    }
}