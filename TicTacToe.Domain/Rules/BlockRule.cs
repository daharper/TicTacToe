using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

public class BlockRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var opponent = board.GetOtherSide(side);
        var squares = board.WinningSquares(opponent);
        return squares.HasItems ? squares.First.Position : null;
    }
}