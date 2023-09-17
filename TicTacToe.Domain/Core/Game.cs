using TicTacToe.Domain.Constants;
using TicTacToe.Infrastructure.Integrity;

namespace TicTacToe.Domain.Core;

public class Game
{
    private readonly List<Position> _moves = new();
    
    public Game(Player player1, Player player2)
    {
        Bob.Expects
            .IsNotNull(player1, "Player one has not been initialised.")
            .IsNotNull(player2, "Player two has not been initialised.")
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
    
    public IEnumerable<Position> Moves => _moves;

    public State State { get; private set; } 
    
    public Player Turn => MoveCount % 2 == 0 ? Player1 : Player2;

    public int MoveCount => _moves.Count;

    public Board Board { get; } = new();

    public Player Player1 { get; set; }
    
    public Player Player2 { get; set; }
    
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