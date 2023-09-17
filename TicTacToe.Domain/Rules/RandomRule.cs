using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

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