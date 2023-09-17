using TicTacToe.Domain.Constants;
using TicTacToe.Infrastructure.Integrity;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Represents a game of Tic Tac Toe.
/// </summary>
public class Game
{
    private readonly List<Position> _moves = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game(Player player1, Player player2)
    {
        Bob.Expects
            .IsNotNull(player1, "Player one has not been initialized.")
            .IsNotNull(player2, "Player two has not been initialized.")
            .IsTrue(player1.Side != Value.Empty, "Player one has not been assigned a side.")
            .IsTrue(player2.Side != Value.Empty, "Player two has not been assigned a side.")
            .IsTrue(player1.Side != player2.Side, "Players cannot have the same side.");
        
        Bob.Assumes
            .IsFalse(player1.Side == player2.Side, "Players cannot have the same side.");
        
        (Player1, Player2) = player1.Side == Value.Cross 
            ?  (player1, player2)
            :  (player2, player1);
        
        State = State.Running;
    }
    
    /// <summary>
    /// Gets the moves made in the game.
    /// </summary>
    public IEnumerable<Position> Moves => _moves;

    /// <summary>
    /// Gets the number of moves made in the game.
    /// </summary>
    public int MoveCount => _moves.Count;

    /// <summary>
    /// Gets the state of the game.
    /// </summary>
    public State State { get; private set; } 
    
    /// <summary>
    /// Gets the player whose turn it is.
    /// </summary>
    public Player Turn => MoveCount % 2 == 0 ? Player1 : Player2;

    /// <summary>
    /// Gets the board.
    /// </summary>
    public Board Board { get; } = new();

    /// <summary>
    /// The player who is playing as crosses.
    /// </summary>
    public Player Player1 { get; set; }
    
    /// <summary>
    /// The player who is playing as noughts.
    /// </summary>
    public Player Player2 { get; set; }
    
    /// <summary>
    /// Moves the player to the specified position.
    /// </summary>
    public Player Move(Player player, Position position)
    {
        Bob.Expects
            .IsTrue(State == State.Running, "Game is not running.")
            .IsTrue(Board.IsAvailable(position), "Position is already taken.")
            .IsTrue(player == Turn, "It is not your turn.");
        
        Board.Move(position, player.Side);
        _moves.Add(position);
        
        State = Board.IsWinningMove(position) 
            ? State.Win
            : Board.IsDraw 
                ? State.Draw 
                : State.Running;

        return player;
    }
}