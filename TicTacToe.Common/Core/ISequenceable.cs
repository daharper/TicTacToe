namespace TicTacToe.Common.Core;

public interface ISequenceable
{
    public bool IsEmpty { get; }

    public bool IsOccupied { get; }
    
    public bool IsSame(ISequenceable other);
}