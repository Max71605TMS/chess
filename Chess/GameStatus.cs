using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class GameStatus
    {
        private static Figure attackingFigure;

        public static bool IsMate(bool isWhiteTurn, IEnumerable<Figure> figures)
        {

            bool canEatAttackingFigure = CanEatAttackingFigure(isWhiteTurn, figures);
            bool canKingMove = IsKingAbleToMove(isWhiteTurn, figures);
            bool isPossibleToProtectKing = IsPossibleToProtectKing(isWhiteTurn, figures);

            if (!canEatAttackingFigure && !canKingMove && !isPossibleToProtectKing)
            { return true; }
            else { return false; }

        }

        public static bool IsStalemate(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            bool canFiguresMove = AllFiguresCanMove(isWhiteTurn, figures);

            if (!canFiguresMove)
            {
                return true;
            }
            else { return false; }
        }

        private static King FindKing(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = (King)figures.First(f => (f is King && f.IsWhite == isWhiteTurn));
            return king;
        }

        // проверка на шах, находим атакующую фигуру
        public static bool IsCheck(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool isCheck = false;

            IEnumerable<Figure> noTurnFigures = figures.Where(f => f.IsWhite != isWhiteTurn).ToList();
            foreach (Figure figure in noTurnFigures)
            {
                IEnumerable<Point> availablePositionsNoTurnFigures = figure.GetAvaliablePositions(figures);
                foreach (var position in availablePositionsNoTurnFigures)
                {
                    if (position == king.Position)
                    {
                        isCheck = true;
                        attackingFigure = figure;
                        break;
                    }
                }
            }

            return isCheck;
        }

        // можно ли съесть атакующую фигуру
        private static bool CanEatAttackingFigure(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool canEatAttackingFigure = false;
            IEnumerable<Figure> figuresWithKingColour = figures.Where(f => f.IsWhite == isWhiteTurn).ToList();

            foreach (Figure figure in figuresWithKingColour)
            {
                IEnumerable<Point> CurrentTurnFiguresAvailablePositions = figure.GetAvaliablePositions(figures);
                foreach (var position in CurrentTurnFiguresAvailablePositions)
                {
                    if (attackingFigure.Position == position)
                    {
                        canEatAttackingFigure = true;
                        break;
                    }
                }
            }

            return canEatAttackingFigure;
        }

        // может ли ходить король
        private static bool IsKingAbleToMove(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool canKingMove = true;
            List<Point> kingAvailablePositions = king.GetAvaliablePositions(figures).ToList();
            kingAvailablePositions.Remove(new Point(king.Position.X + 2, king.Position.Y));
            kingAvailablePositions.Remove(new Point(king.Position.X - 2, king.Position.Y));

            if (kingAvailablePositions.Count == 0)
            {
                canKingMove = false;
            }
            return canKingMove;
        }

        //у всех ли фигур текущего хода есть возможность ходить
        private static bool AllFiguresCanMove(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool figuresCanMove = false;

            IEnumerable<Figure> figuresWithKingColour = figures.Where(f => f.IsWhite == isWhiteTurn).ToList();
            foreach (Figure figure in figuresWithKingColour)
            {
                List<Point> CurrentTurnFiguresAvailablePositions = figure.GetAvaliablePositions(figures).ToList();
                if (CurrentTurnFiguresAvailablePositions.Count != 0)
                {
                    figuresCanMove = true;
                    break;
                }
            }

            return figuresCanMove;
        }

        // можно ли защитить короля другой фигурой
        private static bool IsPossibleToProtectKing(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            bool isPossibleToProtectKing = false;
            King king = FindKing(isWhiteTurn, figures);
            
            List<Point> positionsAroundKing = king.AllTheKingMoves(figures);
            positionsAroundKing.Remove(new Point(king.Position.X + 2, king.Position.Y));
            positionsAroundKing.Remove(new Point(king.Position.X - 2, king.Position.Y));

            foreach (var position in positionsAroundKing)
            {
                if (attackingFigure.Position == position)
                { return isPossibleToProtectKing; }
            }

            IEnumerable<Figure> FiguresWhoseTurn = figures.Where(f => f.IsWhite == isWhiteTurn).ToList();
            List<Point> attackFigureAvailablePositions = new List<Point>();
            IEnumerable<Point> attackFigureAllAvailablePositions = attackingFigure.GetAvaliablePositions(figures);

            if (attackingFigure is Knight Knight)
            {
                return isPossibleToProtectKing;
            }
            else if (attackingFigure is Rook)
            {
                attackFigureAvailablePositions = AvailableMovesForRook(king, attackingFigure, attackFigureAllAvailablePositions);
            }
            else if (attackingFigure is Bishop)
            {
                attackFigureAvailablePositions = AvailableMovesForBishop(king, attackingFigure, attackFigureAllAvailablePositions);
            }
            else if (attackingFigure is Queen)
            {
                attackFigureAvailablePositions = AvailableMovesForQueen(king, attackingFigure, attackFigureAllAvailablePositions);
            }

            Func<Point, Point, bool> predicate = (Point1, Point2) => Point1 == Point2;

            foreach (Figure figure in FiguresWhoseTurn)
            {
                IEnumerable<Point> FiguresWhoseTurnAvailablePositions = figure.GetAvaliablePositions(figures);
                foreach (var position in FiguresWhoseTurnAvailablePositions)
                {
                    foreach (var pos in attackFigureAvailablePositions)
                    {
                        if (predicate(position, pos))
                        {
                            isPossibleToProtectKing = true;
                            break;
                        }
                    }
                }
            }

            return isPossibleToProtectKing;
        }

        private static void AddPositios(Func<Point, bool> predicate, Point point, List<Point> attackFigureAP)
        {
            if (predicate(point))
            {
                attackFigureAP.Add(point);
            }
        }

        private static List<Point> AvailableMovesForRook(King king, Figure attackingFigure, IEnumerable<Point> attackFigureAllAP)
        {
            List<Point> attackFigureAP = new List<Point>();

            foreach (var position in attackFigureAllAP)
            {
                AddPositios(position => position.X > king.Position.X && position.X < attackingFigure.Position.X, position, attackFigureAP);
                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X, position, attackFigureAP);
                AddPositios(position => position.Y > king.Position.Y && position.Y < attackingFigure.Position.Y, position, attackFigureAP);
                AddPositios(position => position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y, position, attackFigureAP);
            }

            return attackFigureAP;
        }

        private static List<Point> AvailableMovesForBishop(King king, Figure attackingFigure, IEnumerable<Point> attackFigureAllAP)
        {
            List<Point> attackFigureAP = new List<Point>();

            foreach (var position in attackFigureAllAP)
            {
                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X
                && position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X
                && position.Y < attackingFigure.Position.Y && position.Y > king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X < attackingFigure.Position.X && position.X > king.Position.X
                && position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X < attackingFigure.Position.X && position.X > king.Position.X
                && position.Y < attackingFigure.Position.Y && position.Y > king.Position.Y, position, attackFigureAP);
            }

            return attackFigureAP;
        }

        private static List<Point> AvailableMovesForQueen(King king, Figure attackingFigure, IEnumerable<Point> attackFigureAllAP)
        {
            List<Point> attackFigureAP = new List<Point>();

            foreach (var position in attackFigureAllAP)
            {
                AddPositios(position => position.X > king.Position.X && position.X < attackingFigure.Position.X
                    && position.Y == attackingFigure.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X
                && position.Y == attackingFigure.Position.Y, position, attackFigureAP);
                AddPositios(position => position.Y > king.Position.Y && position.Y < attackingFigure.Position.Y
                && position.X == attackingFigure.Position.X, position, attackFigureAP);
                AddPositios(position => position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y
                && position.X == attackingFigure.Position.X, position, attackFigureAP);

                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X
                && position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X > attackingFigure.Position.X && position.X < king.Position.X
                && position.Y < attackingFigure.Position.Y && position.Y > king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X < attackingFigure.Position.X && position.X > king.Position.X
                && position.Y > attackingFigure.Position.Y && position.Y < king.Position.Y, position, attackFigureAP);
                AddPositios(position => position.X < attackingFigure.Position.X && position.X > king.Position.X
                && position.Y < attackingFigure.Position.Y && position.Y > king.Position.Y, position, attackFigureAP);
            }

            return attackFigureAP;
        }
    }
}
