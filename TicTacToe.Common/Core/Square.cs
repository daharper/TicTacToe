using TicTacToe.Common.Constants;

namespace TicTacToe.Common.Core;

/// <summary>
/// Represents a square on the board.
/// </summary>
public class Square : ISequenceable
{
    private const string IdPrefix = "sq";

    /// <summary>
    /// Initializes a new instance of square with the specified position and value.
    /// </summary>
    public Square(Position position, Value value = Value.Empty)
    {
        Position = position;
        Value = value;
        Id = $"{IdPrefix}{Index}";
    }
    
    #region properties
    
    public Position Position { get;  }
    public Value Value { get; set; }
    public int Index => (int)Position;
    public string Id { get; private set; }
    public int Row => Index / 3;
    public int Column => Index % 3;
    public bool IsCross() => Value == Value.Cross;
    public bool IsNought() => Value == Value.Nought;
    public bool IsCorner => Index is 0 or 2 or 6 or 8;
    public bool IsEdge => Index is 1 or 3 or 5 or 7;
    public bool IsCenter => Index == 4;
    public bool IsEmpty => Value == Value.Empty;
    public bool IsOccupied => Value != Value.Empty;
    
    #endregion
    
    public bool IsSame(ISequenceable other) => Value == ((Square)other).Value;
    
    public override string ToString() => Value switch
    {
        Value.Nought => "O",
        Value.Cross => "X",
        _ => " "
    };  
}