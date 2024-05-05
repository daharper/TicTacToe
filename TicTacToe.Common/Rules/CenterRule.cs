using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to capture the center.
/// </summary>
public class CenterRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        return board[Position.MiddleCenter] == Value.Empty 
            ? Position.MiddleCenter
            : null;
    }
}