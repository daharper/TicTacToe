using TicTacToe.Domain.Constants;
using TicTacToe.Infrastructure.Integrity;
using TicTacToe.Infrastructure.Utils;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Represents a line of 3 squares.
/// </summary>
public class Line : Sequence<Square>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Line"/> class.
    /// </summary>
    public Line(Square first, Square second, Square third) : base(first, second, third) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Line"/> class.
    /// Expects a sequence of 3 squares, otherwise an exception is thrown.
    /// </summary>
    public Line(Sequence<Square> other) : base(other[0], other[1], other[2])
        => Bob.Assumes.IsEqual(3, other.Length, "A line must have 3 squares");
    
    /// <summary>
    /// Determines if the line is a winning line.
    /// </summary>
    public bool IsWin => IsFull && IsSame;

    /// <summary>
    /// Determines if the line is one step from a win.
    /// </summary>
    public bool IsWinnable(Value value) => EmptyCount == 1 && CountOf(s => s.Value == value) == 2;
}