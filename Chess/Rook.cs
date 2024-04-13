using Chess.Interfaces;
using Chess.Properties;

namespace Chess;

public class Rook : Figure, IMarkable, IMoveByDirection, ICastling
{
    public Rook(bool isWhite, Point point) : base(isWhite, point)
    {
    }

    public bool IsFirstTurn { get; set; } = true;

    public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
    {
        var positions = new List<Point>();

        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, -1, 0)); // По горизонтали слева
        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, 1, 0)); // По горизонтали справа
        positions.AddRange(
            ((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, -1)); // По вертикали сверху
        positions.AddRange(((IMoveByDirection)this).GetPositionsByDirection(figures, this, 0, 1)); // По вертикали снизу

        if (!IsFirstTurn || !isChoosen) return positions;

        positions.AddRange(((ICastling)this).GetCastlingPositionsByDirection(figures, this, -1));
        positions.AddRange(((ICastling)this).GetCastlingPositionsByDirection(figures, this, 1));

        return positions;
    }

    public override Image GetImage()
    {
        if (isWhite)
        {
            if (isChoosen)
                return Resources.Castle_White_Green;

            return (Position.X + Position.Y) % 2 == 0 ? Resources.Castle_White_White : Resources.Castle_White_Black;
        }

        if (isChoosen)
            return Resources.Castle_Black_Green;
        return (Position.X + Position.Y) % 2 == 0 ? Resources.Castle_Black_White : Resources.Castle_Black_Black;
    }
}