namespace TicTacToe.Infrastructure.Utils;

public interface ISequenceValue
{
    public bool IsEmpty { get; }

    public bool IsOccupied { get; }
    
    public bool IsSame(ISequenceValue other);
}