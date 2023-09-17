using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

public class FirstMoveRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        if (moveCount > 1) return null;
        
        var position = new CenterRule().Execute(moveCount, board, side);
        return position ?? new RandomCornerRule().Execute(moveCount, board, side);
    }
}