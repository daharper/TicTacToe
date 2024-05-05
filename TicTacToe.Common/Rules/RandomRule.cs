using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Makes a random move.
/// </summary>
public class RandomRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var available = board.EmptySquares;
        
        var random = new Random();
        var index = random.Next(available.Length);

        return available[index].Position;
    }
}