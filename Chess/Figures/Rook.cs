using Chess.Abstract;
using Chess.Interfaces;
using Chess.Properties;
using Chess.Visuals;

namespace Chess.Figures;

public class Rook : Figure, IFigureRestriction, IMoveByDirection, ICastling
{
    public Rook(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, 0)); // По горизонтали слева
        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, 0)); // По горизонтали справа
        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, -1)); // По вертикали сверху
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, 1)); // По вертикали снизу

        if (!IsFirstTurn || !IsSelected) return positions;

        var castlingPositions = new List<Point>();

        castlingPositions.AddRange(((ICastling)this).GetCastlingFiguresByDirection(figures, this, -1));
        castlingPositions.AddRange(((ICastling)this).GetCastlingFiguresByDirection(figures, this, 1));

        if (castlingPositions.Count > 0)
            positions.AddRange(((ICastling)this).CheckCastlingPositions(figures, this, castlingPositions));

        return positions;
    }

    public override (Color color, Image image) GetVisuals()
    {
        if (IsWhite)
        {
            if (IsSelected)
                return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.RookWhite);
            return (Position.X + Position.Y) % 2 == 0
                ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.RookWhite)
                : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.RookWhite);
        }

        if (IsSelected)
            return (ElementColors.GetElementColor(ElementColor.Green, Position), ChessResources.RookBlack);
        return (Position.X + Position.Y) % 2 == 0
            ? (ElementColors.GetElementColor(ElementColor.White, Position), ChessResources.RookBlack)
            : (ElementColors.GetElementColor(ElementColor.Black, Position), ChessResources.RookBlack);
    }
}