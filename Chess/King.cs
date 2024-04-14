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

            var blackRook = figures.Where(f => f is Rook).Where(f => f.IsWhite == false);
            var whiteRook = figures.Where(f => f is Rook).Where(f => f.IsWhite);

            //allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
            //allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));

            List<Point> getAvaliablePositions = new List<Point>();

            if (IsWhite)
            {
                if(whiteRook != null && IsFirstTurn)
                {
                  
                    
                }

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

        private List<Point> AllTheKingMoves(IEnumerable<Figure> figures)
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
