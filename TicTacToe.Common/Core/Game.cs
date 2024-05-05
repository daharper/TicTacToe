using TicTacToe.Common.Constants;

namespace TicTacToe.Common.Core;

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
        if (State != State.Running) 
            throw new InvalidOperationException("Game is not running.");

        if (Board[position] != Value.Empty)
            throw new InvalidOperationException("Position is already taken.");
        
        if (player != Turn)
            throw new InvalidOperationException("It is not the player's turn.");
        
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