using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

public abstract class Rule
{
    public abstract Position? Execute(int moveCount, Board board, Value side);
}