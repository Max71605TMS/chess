namespace Chess.Abstract;

public abstract class Figure
{
    public Figure(bool isWhite, Point point)
    {
        IsWhite = isWhite;
        Position = point;
    }

    public Point Position { get; set; }

    public bool IsSelected { get; set; }

    public bool IsWhite { get; set; }

    public abstract IEnumerable<Point> GetAvailablePositions(IEnumerable<Figure> figures);

    public abstract (Color color, Image image) GetVisuals();
}