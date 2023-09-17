using TicTacToe.Domain.Constants;
using TicTacToe.Infrastructure.Integrity;
using TicTacToe.Infrastructure.Utils;

namespace TicTacToe.Domain.Core;

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
    
    public bool IsWin => IsFull && IsSame;

    public bool IsWinnable(Value value) => EmptyCount == 1 && CountOf(s => s.Value == value) == 2;

    public Sequence<Square> Filter(Value value) => Filter(s => s.Value == value);
    
    public Sequence<Square> Crosses => Filter(Value.Cross);
    
    public Sequence<Square> Noughts => Filter(Value.Nought);

    public bool IsEmptyOrCrosses => CountOf(s => s.IsEmpty || s.Value == Value.Cross) == 3;
    
    public bool IsEmptyOrNoughts => CountOf(s => s.IsEmpty || s.Value == Value.Nought) == 3;
}