namespace TicTacToe.Infrastructure.Utils;

public interface ISequenceable
{
    public bool IsEmpty { get; }

    public bool IsOccupied { get; }
    
    public bool IsSame(ISequenceable other);
}