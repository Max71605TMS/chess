using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class King : Figure
    {
        private const int BoardSize = 8;

        public bool _isInInitialPositon = true;

        public King(bool isWhite, Point point) : base(isWhite, point)
        {
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {
            var allTheKingMoves = AllTheKingMoves();
            return allTheKingMoves.Where(p => p.X >= 0 && p.X < BoardSize && p.Y >= 0 && p.Y < BoardSize);
        }

        public override Image GetImage()
        {
            var imageName = isWhite
                ? (isChoosen ? "King_White_Green" : (Position.X + Position.Y) % 2 == 0 ? "King_White_White" : "King_White_Black")
                : (isChoosen ? "King_Black_Green" : (Position.X + Position.Y) % 2 == 0 ? "King_Black_White" : "King_Black_Black");

            return (Image)Properties.Resources.ResourceManager.GetObject(imageName);
        }

        private List<Point> AllTheKingMoves()
        {
            var allTheKingMoves = new List<Point>();

            var point = new Point(Position.X, Position.Y + 1);
            if (Position.Y < BoardSize - 1 )
            {
                new Point(Position.X, Position.Y + 1),
                new Point(Position.X, Position.Y - 1),
                new Point(Position.X + 1, Position.Y),
                new Point(Position.X - 1, Position.Y),
                new Point(Position.X + 1, Position.Y - 1),
                new Point(Position.X + 1, Position.Y + 1),
                new Point(Position.X - 1, Position.Y + 1),
                new Point(Position.X - 1, Position.Y - 1)
            };

            if (_isInInitialPositon /*&& Castle._isInInitialPositon*/)
            {
                allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
                allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
            }

            return allTheKingMoves;
        }
    }
}
