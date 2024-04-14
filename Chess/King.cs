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

            var attackBlackFiguresPositions = blackFiguresPositions.Where(f => !(f is Pawn))
                                                                  .Where(f => !(f is King))
                                                                  .Select(position => position.GetAvaliablePositions(figures))
                                                                  .SelectMany(p => p).ToList();

            var attackWhiteFiguresPositions = whiteFiguresPositions.Where(f => !(f is Pawn))
                                                                 .Where(f => !(f is King))
                                                                 .Select(position => position.GetAvaliablePositions(figures))
                                                                 .SelectMany(p => p).ToList();


            List<Point> getAvaliablePositions = new List<Point>();
            if (IsWhite)
            {
                foreach (var move in allTheKingMoves)
                {
                    
                    if (whiteFiguresPositions.All(f => f.Position != move) && attackBlackFiguresPositions.All(attack => attack != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }

            if (!IsWhite)
            {
                foreach (var move in allTheKingMoves)
                {
                    
                    if (blackFiguresPositions.All(f => f.Position != move) && attackWhiteFiguresPositions.All(attack => attack != move))
                    {
                        getAvaliablePositions.Add(move);
                    }
                }
            }
            return getAvaliablePositions;
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
        /// <summary>
        /// Все ходы короля без учета фигур
        /// </summary>
        /// <param name="figures"></param>
        /// <returns></returns>
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

            //ракеровка! добавить поля в Castle с _isInInitialPositon в проверку

            if (IsFirstTurn)
            {
                allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
            }

            if (IsFirstTurn)
            {
                allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
            }

            return allTheKingMoves;
        }

    }
}
