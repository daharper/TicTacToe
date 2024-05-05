using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to block a winning move.
/// </summary>
public class BlockRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var opponent = board.GetOtherSide(side);
        var squares = board.WinningSquares(opponent);

        return squares.HasItems ? squares.First.Position : null;
    }
}