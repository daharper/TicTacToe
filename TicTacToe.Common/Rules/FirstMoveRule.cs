using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// First move only: tries to capture center, otherwise takes a random corner.
/// </summary>
public class FirstMoveRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        if (moveCount > 1) return null;
        
        var position = new CenterRule().Execute(moveCount, board, side);

        return position ?? new RandomCornerRule().Execute(moveCount, board, side);
    }
}