using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

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