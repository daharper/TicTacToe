using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Uses the minimax algorithm to determine the move.
/// </summary>
public class MinimaxRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var (row, col) = Analyzer.Execute(board, side);

        return board.PositionFrom(row, col);
    }
}

