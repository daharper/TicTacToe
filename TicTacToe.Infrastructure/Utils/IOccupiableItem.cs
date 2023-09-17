namespace TicTacToe.Infrastructure.Utils;

public interface IOccupiableItem
{
    public bool IsEmpty { get; }

    public bool IsOccupied { get; }
    
    public bool IsSame(IOccupiableItem other);
}