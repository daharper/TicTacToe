using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Base class for all computer players.
/// </summary>
public abstract class ComputerPlayer : Player
{
    private readonly List<Rule> _rules = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ComputerPlayer"/> class.
    /// </summary>
    protected ComputerPlayer(string name, Value side) : base(name, side)
    {
    }

    /// <summary>
    /// Selects the next move for the computer player by executing rules.
    /// </summary>
    public override Position SelectMove(int moveCount, Board board)
    {
        foreach (var rule in _rules)
        {
            var move = rule.Execute(moveCount, board, Side);
            if (move is not null) return move.Value;
        }
        
        throw new InvalidOperationException("Player cannot select a move.");
    }

    /// <summary>
    /// Adds a rule for consideration when determining a move.
    /// </summary>
    protected void AddRule<T>() where T : Rule, new() => _rules.Add(new T());
}