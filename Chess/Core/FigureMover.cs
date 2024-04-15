using Chess.Abstract;
using Chess.Figures;
using Chess.Interfaces;

namespace Chess.Core;

public class FigureMover
{
    public bool IsWhiteTurn { get; private set; } = true;

    public IEnumerable<Point>? AvailablePositions { get; private set; }

    public Figure? CurrentFigure { get; private set; }

    public (bool isCheck, bool isMate, bool isWhite)? CheckMateStatus { get; private set; }

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
                switch (CurrentFigure)
                {
                    case King when attackedFigure.Position.X < CurrentFigure.Position.X:
                        CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X - 2 };
                        attackedFigure.Position = CurrentFigure.Position with { X = attackedFigure.Position.X + 3 };
                        break;
                    case King:
                        CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X + 2 };
                        attackedFigure.Position = attackedFigure.Position with { X = attackedFigure.Position.X - 2 };
                        break;
                    case Rook when attackedFigure.Position.X < CurrentFigure.Position.X:
                        CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X - 2 };
                        attackedFigure.Position = CurrentFigure.Position with { X = attackedFigure.Position.X + 2 };
                        break;
                    case Rook:
                        CurrentFigure.Position = CurrentFigure.Position with { X = CurrentFigure.Position.X + 3 };
                        attackedFigure.Position = attackedFigure.Position with { X = attackedFigure.Position.X - 2 };
                        break;
                }

                ((IFigureRestriction)attackedFigure).IsFirstTurn = false;

                return;
            }

            //Если разные цвета, то атака
            Figures.Remove(attackedFigure);
        }

        CurrentFigure.Position = targetPosition;

        CheckMateStatus = CheckCheckmate();

        SwitchTurn();
    }

    //Смена хода
    private void SwitchTurn()
    {
        IsWhiteTurn = !IsWhiteTurn;
    }

    //Проверка на шах и мат
    private (bool isCheck, bool isMate, bool isWhite)? CheckCheckmate()
    {
        var king = Figures.First(f => f is King king && king.IsWhite != IsWhiteTurn);

        var allEnemyFiguresAvailablePositions = Figures.Where(w => w.IsWhite == IsWhiteTurn)
            .Select(s => s.GetAvailablePositions(Figures)).SelectMany(s => s).ToList();

        var isCheck = allEnemyFiguresAvailablePositions.Contains(king.Position);

        if (!isCheck)
            return null;

        var kingAvailablePositions =
            king.GetAvailablePositions(Figures).Except(allEnemyFiguresAvailablePositions).ToList();

        var isMate = kingAvailablePositions.Count == 0;

        return new ()
        {
            isCheck = isCheck,
            isMate = isMate,
            isWhite = king.IsWhite
        };
    }
}