using Chess.Abstract;
using Chess.Core;
using Chess.Enums;
using Chess.Figures;
using Chess.Visuals;

namespace Chess;

public partial class Chess : Form
{
    private const int BoardSize = 8;
    private const int ButtonSize = 75;

    private readonly Button[,] _chessButtons = new Button[BoardSize, BoardSize];

    private readonly FigureMover _figureMover;

    public Chess()
    {
        _figureMover = new FigureMover();
        InitializeComponent();
        InitializeChessBoard();
    }

    private void InitializeChessBoard()
    {
        for (var y = 0; y < BoardSize; y++)
        for (var x = 0; x < BoardSize; x++)
        {
            var button = Initializer.GetButton(ButtonSize, x, y, 60, 20);
            _chessButtons[x, y] = button;
            Controls.Add(_chessButtons[x, y]);

            _chessButtons[x, y].Click += ChessButton_Click;

            var horizontalLabels = new Dictionary<int, string>
                { { 7, "H" }, { 6, "G" }, { 5, "F" }, { 4, "E" }, { 3, "D" }, { 2, "C" }, { 1, "B" }, { 0, "A" } };
            var verticalLabels = new Dictionary<int, string>
                { { 0, "8" }, { 1, "7" }, { 2, "6" }, { 3, "5" }, { 4, "4" }, { 5, "3" }, { 6, "2" }, { 7, "1" } };

            if (y == 7 && horizontalLabels.TryGetValue(x, out var horizontalLabel))
            {
                SetLabelOnBoard(LabelDirection.Horizontal, button, horizontalLabel);
            }

            if (x != 0) continue;
            
            if (verticalLabels.TryGetValue(y, out var verticalLabel))
            {
                SetLabelOnBoard(LabelDirection.Vertical, button, verticalLabel);
            }
        }

        SetFigures();
    }

    //Установка букв и цифр рядом с доской
    private void SetLabelOnBoard(LabelDirection direction, Button button, string labelText)
    {
        Label label;

        switch (direction)
        {
            case LabelDirection.Vertical:
                label = Initializer.GetLabel(button, labelText);
                label.Location = label.Location with
                {
                    X = label.Location.X - label.Size.Width - 10,
                    Y = label.Location.Y + button.Size.Height / 2 - label.Size.Height / 2
                };
                Controls.Add(label);
                label.BringToFront();
                break;
            case LabelDirection.Horizontal:
                label = Initializer.GetLabel(button, labelText);
                label.Location = label.Location with
                {
                    X = label.Location.X + button.Size.Width / 2 - label.Size.Width / 2,
                    Y = label.Location.Y + button.Size.Height + 10
                };
                Controls.Add(label);
                label.BringToFront();
                break;
        }
    }

    //Ивент нажатия на кнопку поля
    private void ChessButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;

        if (button.Tag is Figure figure)
        {
            if (figure.IsWhite == _figureMover.IsWhiteTurn)
            {
                //Если не выбрана фигура
                if (_figureMover.CurrentFigure is null)
                {
                    _figureMover.SelectCurrentFigure(figure);

                    SetFigures();

                    if (_figureMover.AvailablePositions is not null)
                        MarkAvailablePositions(_figureMover.AvailablePositions);

                    return;
                }

                //Если нажатие на одну и ту же фигуру
                if (figure.Position == _figureMover.CurrentFigure?.Position)
                {
                    _figureMover.DeselectCurrentFigure();

                    SetFigures();

                    return;
                }

                //Если на другую фигуру того же цвета

                //Рокировка фигуры
                if ((figure is Rook { IsFirstTurn: true } &&
                     _figureMover.CurrentFigure is King { IsFirstTurn: true }) ||
                    (figure is King { IsFirstTurn: true } && _figureMover.CurrentFigure is Rook { IsFirstTurn: true }))
                {
                    _figureMover.MoveFigure(figure.Position);
                    _figureMover.DeselectCurrentFigure();

                    SetFigures();

                    return;
                }

                //Перевыбор фигуры
                _figureMover.DeselectCurrentFigure();
                _figureMover.SelectCurrentFigure(figure);

                SetFigures();

                if (_figureMover.AvailablePositions is not null)
                    MarkAvailablePositions(_figureMover.AvailablePositions);

                return;
            }

            //Атака вражеской фигуры
            _figureMover.MoveFigure(figure.Position);
            _figureMover.DeselectCurrentFigure();

            SetFigures();

            return;
        }

        //Нажатие в пустом месте. Передвижение или отмена выделения
        if (button.Tag is not Point point) return;

        _figureMover.MoveFigure(point);
        _figureMover.DeselectCurrentFigure();

        SetFigures();
    }

    //Установка информации о фигурах в кнопки
    private void SetFigures()
    {
        ResetAllButtonInformation();
        foreach (var figure in _figureMover.Figures) SetFigureButtonInformation(figure);
    }

    //Сброс информации в кнопке
    private void ResetAllButtonInformation()
    {
        for (var y = 0; y < BoardSize; y++)
        for (var x = 0; x < BoardSize; x++)
        {
            _chessButtons[x, y].Tag = new Point(x, y);
            _chessButtons[x, y].BackgroundImage = null;
            _chessButtons[x, y].BackColor = (x + y) % 2 == 0
                ? ElementColors.GetElementColor(ElementColor.White, new Point(x, y))
                : ElementColors.GetElementColor(ElementColor.Black, new Point(x, y));
        }
    }

    //Подсветить доступные ходы
    private void MarkAvailablePositions(IEnumerable<Point> positions)
    {
        foreach (var position in positions)
        {
            var figure = _figureMover.Figures.FirstOrDefault(figure => figure.Position == position);

            if (figure is not null)
            {
                SetFigureButtonInformation(figure, true);

                continue;
            }

            SetEmptyButtonInformation(position);
        }
    }

    //Задание информации кнопке о фигуре
    private void SetFigureButtonInformation(Figure figure, bool isBattle = false)
    {
        var visuals = figure.GetVisuals();
        var color = isBattle
            ? figure.IsWhite == _figureMover.IsWhiteTurn
                ? figure is Rook or King
                    ? ElementColors.GetElementColor(ElementColor.Castling, figure.Position)
                    : ElementColors.GetElementColor(ElementColor.Green, figure.Position)
                : ElementColors.GetElementColor(ElementColor.Red, figure.Position)
            : visuals.color;

        SetButtonProperties(figure.Position.X, figure.Position.Y, color, figure, visuals.image);
    }

    //Задание информации кнопке о пустом месте
    private void SetEmptyButtonInformation(Point position)
    {
        SetButtonProperties(position.X, position.Y, ElementColors.GetElementColor(ElementColor.Green, position));
    }

    //Задание основной информации и стиля для кнопки
    private void SetButtonProperties(int x, int y, Color color, Figure? figure = null, Image? image = null)
    {
        _chessButtons[x, y].Tag = figure is not null ? figure : new Point(x, y);
        _chessButtons[x, y].BackgroundImage = image;
        _chessButtons[x, y].BackColor = color;
    }
}