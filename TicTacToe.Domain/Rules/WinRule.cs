using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

public class WinRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var squares = board.WinningSquares(side);
        return squares.HasItems ? squares.First.Position : null;
    }
}