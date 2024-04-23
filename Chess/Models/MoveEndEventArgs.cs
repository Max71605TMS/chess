using Chess.Abstract;

namespace Chess.Models;

public class MoveEndEventArgs
{
    public bool IsCheck { get; init; }
    public bool IsMate { get; init; }
    public bool KingColor { get; init; }
    public Figure? ThreatFigure { get; init; }
}