using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to make a winning move.
/// </summary>
public class WinRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var squares = board.WinningSquares(side);

        return squares.HasItems ? squares.First.Position : null;
    }
}