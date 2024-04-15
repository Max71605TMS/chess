using Chess.Abstract;
using Chess.Interfaces;

namespace Chess.Core;

public class FigureMover
{
    public bool IsWhiteTurn { get; private set; } = true;

    public IEnumerable<Point>? AvailablePositions { get; private set; }

    public Figure? CurrentFigure { get; private set; }

    public List<Figure> Figures { get; } = Initializer.GetFigures();

    //Выбор фигуры
    public void SelectCurrentFigure(Figure figure)
    {
        CurrentFigure = figure;
        CurrentFigure.IsSelected = true;

        GetAvailablePositions(figure);
    }

    //Получение возможных ходов для фигуры
    private void GetAvailablePositions(Figure figure)
    {
        AvailablePositions = figure.GetAvailablePositions(Figures);
    }

    //Сброс выделения и информации о выбранной фигуре
    public void DeselectCurrentFigure()
    {
        if (CurrentFigure is null) return;

        CurrentFigure.IsSelected = false;
        CurrentFigure = null;
        AvailablePositions = null;
    }

    //Движение фигур
    public void MoveFigure(Point targetPosition)
    {
        if (CurrentFigure is null || AvailablePositions is null) return;

        if (AvailablePositions.All(position => position != targetPosition)) return;

        //Если фигура имеет ограничения после 1го хода
        if (CurrentFigure is IFigureRestriction figure) figure.IsFirstTurn = false;

        var attackedFigure = Figures.FirstOrDefault(f => f.Position == targetPosition);

        if (attackedFigure is not null)
        {
            //Если обе фигуры белые, то рокировка
            if (attackedFigure.IsWhite == CurrentFigure.IsWhite)
            {
                if (attackedFigure.Position.X < CurrentFigure.Position.X)
                {
                    CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X - 2 };
                    attackedFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X + 1 };
                }
                else
                {
                    CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X + 3 };
                    attackedFigure.Position = attackedFigure.Position with { X = attackedFigure.Position.X - 2 };
                }

                ((IFigureRestriction)attackedFigure).IsFirstTurn = false;

                SwitchTurn();
                return;
            }

            //Если разные цвета, то атака
            Figures.Remove(attackedFigure);
        }

        CurrentFigure.Position = targetPosition;

        SwitchTurn();
    }

    //Смена хода
    private void SwitchTurn()
    {
        IsWhiteTurn = !IsWhiteTurn;
    }
}