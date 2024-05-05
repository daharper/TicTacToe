using TicTacToe.Common.Constants;

namespace TicTacToe.Common.Core;

/// <summary>
/// Computer players use rules to determine their next move.
/// </summary>
public abstract class Rule
{
    public abstract Position? Execute(int moveCount, Board board, Value side);
}