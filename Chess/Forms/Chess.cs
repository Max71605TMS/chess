using Chess.Abstract;
using Chess.Core;
using Chess.Enums;
using Chess.Figures;
using Chess.Visuals;

namespace Chess.Forms;

public partial class Chess : Form
{
    private const int BoardSize = 8;
    private const int ButtonSize = 75;

    private readonly Button[,] _chessButtons = new Button[BoardSize, BoardSize];
    private readonly FigureMover _mover;
    private PictureBox _moveStatusPicture = new();

    private Label _moveStatusText = new();

    public Chess(FigureMover mover)
    {
        _mover = mover;
        InitializeComponent();
        InitializeChessBoard();
        InitializeTurnInformation();

        SetTurnInfo();
    }

    private void InitializeChessBoard()
    {
        for (var y = 0; y < BoardSize; y++)
        for (var x = 0; x < BoardSize; x++)
        {
            var button = Initializer.GetButton(ButtonSize, x, y, 60, 80);
            _chessButtons[x, y] = button;
            Controls.Add(_chessButtons[x, y]);

            _chessButtons[x, y].Click += ChessButtonClicked;

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

    //Создание элемента где отображается информация о текущем ходе
    private void InitializeTurnInformation()
    {
        var groupBox = new GroupBox
        {
            Location = new Point
            {
                X = 0,
                Y = 0
            },
            Margin = new Padding(0),
            Name = "MoveStatus",
            Size = new Size(150, 50),
            TabIndex = 0,
            TabStop = false,
            Text = @"Текущий ход",
            ForeColor = ElementColors.GetElementColor(ElementColor.White)
        };

        groupBox.Location = groupBox.Location with
        {
            X = groupBox.Location.X + Size.Width / 2 - groupBox.Size.Width / 2,
            Y = groupBox.Location.Y + 35 - groupBox.Size.Height / 2
        };

        _moveStatusText = new Label
        {
            AutoSize = true,
            ForeColor = ElementColors.GetElementColor(ElementColor.White),
            BackColor = Color.Transparent,
            Location = new Point(0, 0),
            Name = "MoveTitle",
            TabIndex = 0
        };

        _moveStatusText.Location = _moveStatusText.Location with
        {
            X = _moveStatusText.Location.X + 10,
            Y = _moveStatusText.Location.Y + groupBox.Size.Height / 2 - 3
        };

        _moveStatusPicture = new PictureBox
        {
            Location = new Point(0, 0),
            Name = "TurnColor",
            Size = new Size(20, 20),
            TabIndex = 3,
            TabStop = false
        };

        _moveStatusPicture.Location = _moveStatusPicture.Location with
        {
            X = _moveStatusPicture.Location.X + groupBox.Size.Width - _moveStatusPicture.Size.Width - 10,
            Y = _moveStatusPicture.Location.Y + 7 + (groupBox.Size.Height - 7) / 2 - _moveStatusPicture.Size.Height / 2
        };

        groupBox.Controls.Add(_moveStatusText);
        groupBox.Controls.Add(_moveStatusPicture);
        Controls.Add(groupBox);
    }

    //Ивент нажатия на кнопку поля
    private void ChessButtonClicked(object? sender, EventArgs e)
    {
        var button = (Button)sender!;

        if (button.Tag is Figure figure)
        {
            if (figure.IsWhite == _mover.IsWhiteTurn)
            {
                var checkMateStatus = _mover.CheckMateStatus;

                if (checkMateStatus is not null && checkMateStatus.Value.isCheck &&
                    checkMateStatus.Value.isWhite == _mover.IsWhiteTurn && figure is not King)
                    return;

                //Если не выбрана фигура
                if (_mover.CurrentFigure is null)
                {
                    _mover.SelectCurrentFigure(figure);

                    SetFigures();

                    if (_mover.AvailablePositions is not null)
                        MarkAvailablePositions(_mover.AvailablePositions);

                    return;
                }

                //Если нажатие на одну и ту же фигуру
                if (figure.Position == _mover.CurrentFigure?.Position)
                {
                    _mover.DeselectCurrentFigure();

                    SetFigures();

                    return;
                }

                //Если на другую фигуру того же цвета

                //Рокировка фигуры
                if ((figure is Rook { IsFirstTurn: true } &&
                     _mover.CurrentFigure is King { IsFirstTurn: true }) ||
                    (figure is King { IsFirstTurn: true } && _mover.CurrentFigure is Rook { IsFirstTurn: true }))
                {
                    _mover.MoveFigure(figure.Position);
                    _mover.DeselectCurrentFigure();

                    SetFigures();
                    SetTurnInfo();

                    return;
                }

                //Перевыбор фигуры
                _mover.DeselectCurrentFigure();
                _mover.SelectCurrentFigure(figure);

                SetFigures();

                if (_mover.AvailablePositions is not null)
                    MarkAvailablePositions(_mover.AvailablePositions);

                return;
            }

            //Атака вражеской фигуры
            _mover.MoveFigure(figure.Position);
            _mover.DeselectCurrentFigure();

            SetFigures();

            if (_mover.CheckMateStatus is not null && _mover.CheckMateStatus.Value.isMate)
            {
                SetTurnInfo(true);
                UnsubClickEvent();
                return;
            }

            SetTurnInfo();

            return;
        }

        //Нажатие в пустом месте. Передвижение или отмена выделения
        if (button.Tag is not Point point) return;

        _mover.MoveFigure(point);
        _mover.DeselectCurrentFigure();

        SetFigures();

        if (_mover.CheckMateStatus is not null && _mover.CheckMateStatus.Value.isMate)
        {
            SetTurnInfo(true);
            UnsubClickEvent();
            return;
        }

        SetTurnInfo();
    }

    //Установка информации о фигурах в кнопки
    private void SetFigures()
    {
        ResetAllButtonInformation();
        foreach (var figure in _mover.Figures) SetFigureButtonInformation(figure);
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
            var figure = _mover.Figures.FirstOrDefault(figure => figure.Position == position);

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
            ? figure.IsWhite == _mover.IsWhiteTurn
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

    //Задание информации в верхнем окне
    private void SetTurnInfo(bool isWin = false)
    {
        var text = isWin
            ? $"Победили {(_mover.CheckMateStatus!.Value.isWhite ? "чёрные" : "белые")}"
            : _mover.IsWhiteTurn
                ? "Ход белых"
                : "Ход чёрных";
        var color = isWin
            ? _mover.CheckMateStatus!.Value.isWhite
                ? Color.Black
                : Color.White
            : _mover.IsWhiteTurn
                ? Color.White
                : Color.Black;

        _moveStatusText.Text = text;
        _moveStatusPicture.BackColor = color;

        if (isWin) MessageBox.Show(text);
    }

    //Отписываемся от всех ивентов кнопок
    private void UnsubClickEvent()
    {
        foreach (var button in _chessButtons) button.Click -= ChessButtonClicked;
    }
}