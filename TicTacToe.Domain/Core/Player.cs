using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

public abstract class Player
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    protected Player(string name, Value side) 
        => (Name, Side) = (name, side);
    
    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the celebration of the player.
    /// </summary>
    public string Celebration { get; set; } = "Woohoo!";
    
    /// <summary>
    /// The side the player is playing as, either Cross or Nought.
    /// </summary>
    public Value Side { get; set;  }
    
    /// <summary>
    /// Selects a move for the player.
    /// </summary>
    public abstract Position SelectMove(int moveCount, Board board);
}