using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class King : Figure
    {
        private const int BoardSize = 8; // размер шахматной доски

        public bool _isInInitialPositon = true; //начальная позиция


        public King(bool isWhite, Point point) : base(isWhite, point)
        {

        }

        public override IEnumerable<Point> GetAvaliablePositions(IEnumerable<Figure> figures) //Получить доступную позицию
        {
            var listFigurePoint = figures.Select(f => f.Position).ToList();  //??????
            var allTheKingMoves = AllTheKingMoves(figures); // все возможные ходы короля без учета фигур
            var howСanHeWalk = new List<Point>();

            foreach (var move in allTheKingMoves)
            {
                foreach (var figure in listFigurePoint)
                {
                    if (move.X != figure.X && move.Y != figure.Y)
                    {
                        if(figures.Any(fig => fig.isWhite))
                        howСanHeWalk.Add(move);
                    }
                }
            }




            return howСanHeWalk;
        }

        public override Image GetImage() // получение картинки фигуры
        {
            if (isWhite)
            {
                if (isChoosen)
                {
                    return Properties.Resources.King_White_Green;
                } else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.King_White_White : Properties.Resources.King_White_Black;
                }
                
            } else
            {
                if (isChoosen)
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
            if (Position.Y + 1 < BoardSize )
            {
                allTheKingMoves.Add(new Point(Position.X, Position.Y + 1));
            }
          
            if (Position.Y - 1 < BoardSize)
            {
                allTheKingMoves.Add(new Point(Position.X, Position.Y - 1));
            }

            if (Position.X + 1 < BoardSize)
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y));
            }

            if (Position.X - 1 < BoardSize)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y));
            }
        
            if (Position.X + 1 < BoardSize && Position.Y  - 1 < BoardSize )
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y - 1));
            }

            if (Position.X + 1 < BoardSize  && Position.Y + 1< BoardSize )
            {
                allTheKingMoves.Add(new Point(Position.X + 1, Position.Y + 1));
            }

            if (Position.X - 1 < BoardSize && Position.Y + 1< BoardSize)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y + 1));
            }

            if (Position.X - 1< BoardSize && Position.Y - 1< BoardSize)
            {
                allTheKingMoves.Add(new Point(Position.X - 1, Position.Y - 1));
            }

            //ракеровка! добавить поля в Castle с _isInInitialPositon в проверку

            if (_isInInitialPositon /*&& Castle._isInInitialPositon*/)
            {
                allTheKingMoves.Add(new Point(Position.X + 2, Position.Y));
            }

            if (_isInInitialPositon /*&& Castle._isInInitialPositon*/)
            {
                allTheKingMoves.Add(new Point(Position.X - 2, Position.Y));
            }

            return allTheKingMoves;
        }
    }
}
