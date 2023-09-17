using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

public abstract class ComputerPlayer : Player
{
    private readonly List<Rule> _rules = new();
    
    protected ComputerPlayer(string name, Value side) : base(name, side)
    {
    }

    public override Position SelectMove(int moveCount, Board board)
    {
        foreach (var rule in _rules)
        {
            var move = rule.Execute(moveCount, board, Side);
            if (move is not null) return move.Value;
        }
        
        throw new InvalidOperationException("Player cannot select a move.");
    }

    protected void AddRule<T>() where T : Rule, new() 
        => _rules.Add(new T());
    
    protected void AddRule(Rule rule) 
        => _rules.Add(rule);
    
    protected void AddRules(IEnumerable<Rule> rules) 
        => _rules.AddRange(rules);   
}