using Chess.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class King : Figure, IMarkable
    {
        private const int BoardSize = 8;

        public bool IsFirstTurn { get; set; } = true;

        public King(bool isWhite, Point point) : base(isWhite, point)
        {
        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures)
        {

            var allTheKingMoves = AllTheKingMoves(figures);

            var whiteFiguresPositions = figures.Where(f => f.IsWhite);
            var blackFiguresPositions = figures.Where(f => f.IsWhite == false);

            var attackBlackFiguresPositions = AttackOfPiecesOtherThanPawns(blackFiguresPositions);
            var attackWhiteFiguresPositions = AttackOfPiecesOtherThanPawns(whiteFiguresPositions);

            var positionWhiteKing = figures.Where(f => f is King).Where(k => k.IsWhite).Select(f => f.Position).First();
            var positionBlackKing = figures.Where(f => f is King).Where(k => k.IsWhite == false).Select(f => f.Position).First();

            var theKingBlackAttack = AttacKing(positionBlackKing);
            var theKingWhiteAttack = AttacKing(positionWhiteKing);

            List<Point> getAvaliablePositions = new List<Point>();

            if (IsWhite)
            {

                foreach (var move in allTheKingMoves)
                {
                    if (whiteFiguresPositions.All(f => f.Position != move) && attackBlackFiguresPositions.All(attack => attack != move)
                                                                           && theKingBlackAttack.All(attack => attack != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }

            if (!IsWhite)
            {
                foreach (var move in allTheKingMoves)
                {
                    if (blackFiguresPositions.All(f => f.Position != move) && attackWhiteFiguresPositions.All(attack => attack != move)
                                                                           && theKingWhiteAttack.All(attack => attack != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }
            return getAvaliablePositions;
        }

        private List<Point> AttackOfPiecesOtherThanPawns(IEnumerable<Figure> figures)
        {
            var attack = figures.Where(f => !(f is Pawn))
                                            .Where(f => !(f is King))
                                            .Select(position => position.GetAvaliablePositions(figures))
                                            .SelectMany(p => p).ToList();
            return attack;
        }

        public override Image GetImage()
        {
            if (IsWhite)
            {
                if (IsChoosen)
                {
                    return Properties.Resources.King_White_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.King_White_White : Properties.Resources.King_White_Black;
                }

            }
            else
            {
                if (IsChoosen)
                {
                    return Properties.Resources.King_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.King_Black_White : Properties.Resources.King_Black_Black;
                }
            }
        }

        public List<Point> AllTheKingMoves(IEnumerable<Figure> figures)
        {
            var allTheKingMoves = new List<Point>();

            var point = new Point(Position.X, Position.Y);
            if (Position.Y + 1 < BoardSize && Position.Y + 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X, Position.Y + 1));
            }

            if (Position.Y - 1 < BoardSize && Position.Y - 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X, Position.Y - 1));
            }

            if (Position.X + 1 < BoardSize && Position.X + 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y));
            }

            if (Position.X - 1 < BoardSize && Position.X - 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y));
            }

            if (Position.X + 1 < BoardSize && Position.Y - 1 < BoardSize && Position.X + 1 >= 0 && Position.Y - 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y - 1));
            }

            if (Position.X + 1 < BoardSize && Position.Y + 1 < BoardSize && Position.X + 1 >= 0 && Position.Y + 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y + 1));
            }

            if (Position.X - 1 < BoardSize && Position.Y + 1 < BoardSize && Position.X - 1 >= 0 && Position.Y + 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y + 1));
            }

            if (Position.X - 1 < BoardSize && Position.Y - 1 < BoardSize && Position.X - 1 >= 0 && Position.Y - 1 >= 0)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y - 1));
            }

            var blackRooks = figures.Where(f => f is Rook).Where(f => f.IsWhite == false).Select(f => (Rook)f);
            var whiteRooks = figures.Where(f => f is Rook).Where(f => f.IsWhite).Select(f => (Rook)f);
            var whiteFiguresPositions = figures.Where(f => f.IsWhite);
            var blackFiguresPositions = figures.Where(f => f.IsWhite == false);
            var allFiguresPositions = whiteFiguresPositions.Concat(blackFiguresPositions);

            if (IsWhite && whiteRooks != null && IsFirstTurn)
            {
                if (whiteRooks.Count() == 2 && whiteRooks.All(f => f.IsFirstTurn == true)
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 1, Position.Y))
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 2, Position.Y))
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 3, Position.Y))
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 1, Position.Y))
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 2, Position.Y))
                                           && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 3, Position.Y)))

                {
                    allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
                    allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
                }
                else if (whiteRooks.Count() == 2 && whiteRooks.Any(f => f.IsFirstTurn == true) ||
                         whiteRooks.Count() == 1 && whiteRooks.Any(f => f.IsFirstTurn == true))
                {
                    var positionRook = whiteRooks.Where(f => f.IsFirstTurn = true).First();

                    if (positionRook.Position.X == 0)
                    {
                        allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
                    }
                    if (positionRook.Position.X == 7)
                    {
                        allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
                    }
                }

            }
            if (!IsWhite && blackRooks != null && IsFirstTurn)
            {
                bool aa = blackRooks.All(f => f.IsFirstTurn == true);
                if (blackRooks.Count() == 2 && blackRooks.All(f => f.IsFirstTurn == true)
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 1, Position.Y))
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 2, Position.Y))
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X + 3, Position.Y))
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 1, Position.Y))
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 2, Position.Y))
                                          && whiteFiguresPositions.Any(f => f.Position != new Point(Position.X - 3, Position.Y)))

                {
                    allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
                    allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
                }
                else if (blackRooks.Count() == 2 && blackRooks.Any(f => f.IsFirstTurn == true) ||
                         blackRooks.Count() == 1 && blackRooks.Any(f => f.IsFirstTurn == true))
                {
                    var positionRook = blackRooks.Where(f => f.IsFirstTurn = true).First();

                    if (positionRook.Position.X == 0)
                    {
                        allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
                    }
                    if (positionRook.Position.X == 7)
                    {
                        allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
                    }
                }

            }

            return allTheKingMoves;

        }

        private List<Point> AttacKing(Point point)
        {
            var AttacKing = new List<Point>();

            var newPoint = new Point(point.X, point.Y);
            if (point.Y + 1 < BoardSize && point.Y + 1 >= 0)
            {
                AttacKing.Add(new Point(point.X, point.Y + 1));
            }

            if (point.Y - 1 < BoardSize && point.Y - 1 >= 0)
            {
                AttacKing.Add(new Point(point.X, point.Y - 1));
            }

            if (point.X + 1 < BoardSize && point.X + 1 >= 0)
            {
                AttacKing.Add(new Point(point.X + 1, point.Y));
            }

            if (point.X - 1 < BoardSize && point.X - 1 >= 0)
            {
                AttacKing.Add(new Point(point.X - 1, point.Y));
            }

            if (point.X + 1 < BoardSize && point.Y - 1 < BoardSize && point.X + 1 >= 0 && point.Y - 1 >= 0)
            {
                AttacKing.Add(new Point(point.X + 1, point.Y - 1));
            }

            if (point.X + 1 < BoardSize && point.Y + 1 < BoardSize && point.X + 1 >= 0 && point.Y + 1 >= 0)
            {
                AttacKing.Add(new Point(point.X + 1, point.Y + 1));
            }

            if (point.X - 1 < BoardSize && point.Y + 1 < BoardSize && point.X - 1 >= 0 && point.Y + 1 >= 0)
            {
                AttacKing.Add(new Point(point.X - 1, point.Y + 1));
            }

            if (point.X - 1 < BoardSize && point.Y - 1 < BoardSize && point.X - 1 >= 0 && point.Y - 1 >= 0)
            {
                AttacKing.Add(new Point(point.X - 1, point.Y - 1));
            }

            return AttacKing;
        }

        //private bool checkAttackPosition(List<Point> attackFigurePosition,List<Point> allTheKingMoves) 
        //{
        //    bool result = false;
        //    foreach (var move in attackFigurePosition)
        //    {
        //        if (attackFigurePosition.All(attack => attack != move)) result = true;
        //    }
        //    return result;
        //}

    }
}
