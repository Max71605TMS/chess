using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Knight : Figure
    {

        private List<Point> _possiblePositions = new List<Point>();

        public Knight(bool isWhite, Point point) : base(isWhite, point)
        {
           
        }

        public override IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures)
        {
            var Positions = new List<Point>();
            bool IsFigureExist;

           _possiblePositions = GetPossiblePositions();


            foreach (var PossiblePosition in _possiblePositions)
            {
                IsFigureExist = figures.Any(f => f.Position == new Point(PossiblePosition.X, PossiblePosition.Y));

                if (IsFigureExist)
                {
                    var figure = figures.First(f => f.Position == new Point(PossiblePosition.X, PossiblePosition.Y));
                    if (this.isWhite != figure.isWhite)
                    {
                        Positions.Add(figure.Position);
                    }
                }
                else
                {
                    if ((PossiblePosition.X >= 0 && PossiblePosition.X < 8) &&
                        (PossiblePosition.Y >= 0 && PossiblePosition.Y < 8))
                    {
                        Positions.Add(new Point(PossiblePosition.X, PossiblePosition.Y));
                    }
                }
            }

            _possiblePositions.Clear();

            return Positions;
        }

        public override Image GetImage()
        {
            if (isWhite)
            {
                if (isChoosen)
                {
                    return Properties.Resources.Knight_White_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Knight_White_White : Properties.Resources.Knight_White_Black;
                }

            }
            else
            {
                if (isChoosen)
                {
                    return Properties.Resources.Knight_Black_Green;
                }
                else
                {
                    return (Position.X + Position.Y) % 2 == 0 ? Properties.Resources.Knight_Black_White : Properties.Resources.Knight_Black_Black;
                }
            }

        }


        private List<Point> GetPossiblePositions()
        {
            _possiblePositions.Add(new Point(this.Position.X - 1, this.Position.Y - 2));
            _possiblePositions.Add(new Point(this.Position.X + 1, this.Position.Y - 2));
            _possiblePositions.Add(new Point(this.Position.X - 1, this.Position.Y + 2));
            _possiblePositions.Add(new Point(this.Position.X + 1, this.Position.Y + 2));
            _possiblePositions.Add(new Point(this.Position.X - 2, this.Position.Y - 1));
            _possiblePositions.Add(new Point(this.Position.X - 2, this.Position.Y + 1));
            _possiblePositions.Add(new Point(this.Position.X + 2, this.Position.Y - 1));
            _possiblePositions.Add(new Point(this.Position.X + 2, this.Position.Y + 1));
            return _possiblePositions;   
        }

    }
}
