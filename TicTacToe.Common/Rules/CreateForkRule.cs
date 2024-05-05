using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to create a fork.
/// </summary>
public class CreateForkRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var opp = board.GetOtherSide(side);
        
        foreach (var line in board.AllLines)
        {
            if (line.CountOf(l => l.Value == opp) == 0 && line.EmptyCount > 0)
            {
                return line.FirstEmpty!.Position;
            }
            
        }
        
        return null;        
    }
}