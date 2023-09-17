using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules;

public class RandomCornerRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var corners = board.EmptyCorners;
        if (corners.HasNoItems) return null;
        
        var random = new Random();
        var index = random.Next(corners.Length);
        
        return corners[index].Position;
    }
}