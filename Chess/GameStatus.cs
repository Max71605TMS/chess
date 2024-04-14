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
            if (isWhiteTurn)
            {
                King king = (King)figures.First(f => (f is King && f.IsWhite));
                return king;
            }
            else
            {
                King king = (King)figures.First(f => (f is King && !f.IsWhite));
                return king;
            }
        }

        // проверка на шах, находим атакующую фигуру
        public static bool IsCheck(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool isCheck = false;

            if (king.IsWhite)
            {
                IEnumerable<Figure> blackFigures = figures.Where(f => !f.IsWhite).ToList();
                foreach (Figure figure in blackFigures)
                {
                    IEnumerable<Point> availableBlackPositions = figure.GetAvaliablePositions(figures);
                    foreach (var position in availableBlackPositions)
                    {
                        if (position == king.Position)
                        {
                            isCheck = true;
                            attackingFigure = figure;
                            break;
                        }
                    }
                }
            }

            if (!king.IsWhite)
            {
                IEnumerable<Figure> whiteFigures = figures.Where(f => f.IsWhite).ToList();
                foreach (Figure figure in whiteFigures)
                {
                    IEnumerable<Point> availableWhitePositions = figure.GetAvaliablePositions(figures);
                    foreach (var position in availableWhitePositions)
                    {
                        if (position == king.Position)
                        {
                            isCheck = true;
                            attackingFigure = figure;
                            break;
                        }
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
            if (king.IsWhite)
            {
                IEnumerable<Figure> whiteFigures = figures.Where(f => f.IsWhite).ToList();
                foreach (Figure figure in whiteFigures)
                {
                    IEnumerable<Point> availableWhitePositions = figure.GetAvaliablePositions(figures);
                    foreach (var position in availableWhitePositions)
                    {
                        if (attackingFigure.Position == position)
                        {
                            canEatAttackingFigure = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                IEnumerable<Figure> blackFigures = figures.Where(f => !f.IsWhite).ToList();
                foreach (Figure figure in blackFigures)
                {
                    IEnumerable<Point> availableBlackPositions = figure.GetAvaliablePositions(figures);
                    foreach (var position in availableBlackPositions)
                    {
                        if (attackingFigure.Position == position)
                        {
                            canEatAttackingFigure = true;
                            break;
                        }
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

        // можно ли защитить короля другой фигурой
        private static bool IsPossibleToProtectKing(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool isPossibleToProtectKing = false;
            List<Point> positionsAroundKing = king.AllTheKingMoves(figures);
            positionsAroundKing.Remove(new Point(king.Position.X + 2, king.Position.Y));
            positionsAroundKing.Remove(new Point(king.Position.X - 2, king.Position.Y));

            foreach (var position in positionsAroundKing)
            {
                if (attackingFigure.Position == position)
                { return isPossibleToProtectKing; }
            }

            IEnumerable<Figure> FiguresWhoMove = figures.Where(f => f.IsWhite == isWhiteTurn).ToList();
            IEnumerable<Point> attackFigureAP = null;

            if (attackingFigure is Knights Knight)
            { return isPossibleToProtectKing; }
            else if (attackingFigure is Rook)
            {
                attackFigureAP = GetAvailablePositionRookCheck(king, figures);
            }
            else if (attackingFigure is Bishop)
            {
                attackFigureAP = GetAvailablePositionBishopCheck(king, figures);
            }
            else if (attackingFigure is Queen)
            { attackFigureAP = GetAvailablePositionQueenCheck(king, figures); }

            foreach (Figure figure in FiguresWhoMove)
            {
                IEnumerable<Point> FiguresWhoseMoveAP = figure.GetAvaliablePositions(figures);
                foreach (var position in FiguresWhoseMoveAP)
                {
                    foreach (var pos in attackFigureAP)
                    {
                        if (pos == position)
                        {
                            isPossibleToProtectKing = true;
                            break;
                        }
                    }
                }
            }

            return isPossibleToProtectKing;
        }

        //у всех ли фигур текущего хода есть возможность ходить
        private static bool AllFiguresCanMove(bool isWhiteTurn, IEnumerable<Figure> figures)
        {
            King king = FindKing(isWhiteTurn, figures);
            bool figuresCanMove = false;
            if (king.IsWhite)
            {
                IEnumerable<Figure> whiteFigures = figures.Where(f => f.IsWhite).ToList();
                foreach (Figure figure in whiteFigures)
                {
                    List<Point> availableWhitePositions = figure.GetAvaliablePositions(figures).ToList();
                    if (availableWhitePositions.Count != 0)
                    {
                        figuresCanMove = true;
                        break;
                    }
                }
            }
            else
            {
                IEnumerable<Figure> BlackFigures = figures.Where(f => !f.IsWhite).ToList();
                foreach (Figure figure in BlackFigures)
                {
                    List<Point> availableBlackPositions = figure.GetAvaliablePositions(figures).ToList();
                    if (availableBlackPositions.Count != 0)
                    {
                        figuresCanMove = true;
                        break;
                    }
                }
            }
            return figuresCanMove;
        }

        private static IEnumerable<Point> GetAvailablePositionRookCheck(King king, IEnumerable<Figure> figures)
        {
            IEnumerable<Point> attackFigureAllAP = attackingFigure.GetAvaliablePositions(figures);
            List<Point> attackFigureAP = attackFigureAllAP.ToList();

            if (king.Position.X == attackingFigure.Position.X)
            {
                attackFigureAP = RemovePointsVerticalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);
            }

            if (king.Position.Y == attackingFigure.Position.Y)
            {
                attackFigureAP = RemovePointsHorizontalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);
            }
            return attackFigureAP;
        }

        private static IEnumerable<Point> GetAvailablePositionBishopCheck(King king, IEnumerable<Figure> figures)
        {
            IEnumerable<Point> attackFigureAllAP = attackingFigure.GetAvaliablePositions(figures);
            List<Point> attackFigureAP = attackFigureAllAP.ToList();

            attackFigureAP = RemovePointsDiagonalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);

            return attackFigureAP;
        }

        private static IEnumerable<Point> GetAvailablePositionQueenCheck(King king, IEnumerable<Figure> figures)
        {
            IEnumerable<Point> attackFigureAllAP = attackingFigure.GetAvaliablePositions(figures);
            List<Point> attackFigureAP = attackFigureAllAP.ToList();

            if (king.Position.X == attackingFigure.Position.X)
            {
                attackFigureAP = RemovePointsVerticalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);
            }
            else if (king.Position.Y == attackingFigure.Position.Y)
            {
                attackFigureAP = RemovePointsHorizontalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);
            }
            else
            {
                attackFigureAP = RemovePointsDiagonalMove(king, attackingFigure, attackFigureAllAP, attackFigureAP);
            }

            return attackFigureAP;
        }

        private static List<Point> RemovePointsVerticalMove(King king, Figure attackingFigure,
            IEnumerable<Point> attackFigureAllAP, List<Point> attackFigureAP)
        {
            foreach (var f in attackFigureAllAP)
            {
                if (f.X != attackingFigure.Position.X)
                { attackFigureAP.Remove(f); }
            }

            if (king.Position.Y > attackingFigure.Position.Y)
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.Y < attackingFigure.Position.Y)
                    { attackFigureAP.Remove(f); }
                }
            }
            else
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.Y > attackingFigure.Position.Y)
                    { attackFigureAP.Remove(f); }
                }
            }
            return attackFigureAP;
        }

        private static List<Point> RemovePointsHorizontalMove(King king, Figure attackingFigure,
         IEnumerable<Point> attackFigureAllAP, List<Point> attackFigureAP)
        {
            foreach (var f in attackFigureAllAP)
            {
                if (f.Y != attackingFigure.Position.Y)
                { attackFigureAP.Remove(f); }
            }

            if (king.Position.X > attackingFigure.Position.X)
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.X < attackingFigure.Position.X)
                    { attackFigureAP.Remove(f); }
                }
            }
            else
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.X > attackingFigure.Position.X)
                    { attackFigureAP.Remove(f); }
                }
            }
            return attackFigureAP;
        }
        private static List<Point> RemovePointsDiagonalMove(King king, Figure attackingFigure,
         IEnumerable<Point> attackFigureAllAP, List<Point> attackFigureAP)
        {
            if (king.Position.X > attackingFigure.Position.X)
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.X < attackingFigure.Position.X)
                    { attackFigureAP.Remove(f); }
                }

                if (king.Position.Y > attackingFigure.Position.Y)
                {
                    foreach (var f in attackFigureAllAP)
                    {
                        if (f.Y < attackingFigure.Position.Y)
                        { attackFigureAP.Remove(f); }
                    }
                }
                else
                {
                    foreach (var f in attackFigureAllAP)
                    {
                        if (f.Y > attackingFigure.Position.Y)
                        { attackFigureAP.Remove(f); }
                    }
                }
            }
            else
            {
                foreach (var f in attackFigureAllAP)
                {
                    if (f.X > attackingFigure.Position.X)
                    { attackFigureAP.Remove(f); }
                }

                if (king.Position.Y > attackingFigure.Position.Y)
                {
                    foreach (var f in attackFigureAllAP)
                    {
                        if (f.Y < attackingFigure.Position.Y)
                        { attackFigureAP.Remove(f); }
                    }
                }
                else
                {
                    foreach (var f in attackFigureAllAP)
                    {
                        if (f.Y > attackingFigure.Position.Y)
                        { attackFigureAP.Remove(f); }
                    }
                }
            }
            return attackFigureAP;
        }
    }
}
