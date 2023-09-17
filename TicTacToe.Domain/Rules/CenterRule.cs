using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

public class CenterRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
        => board[Position.MiddleCenter] == Value.Empty 
            ? Position.MiddleCenter 
            : null;
}