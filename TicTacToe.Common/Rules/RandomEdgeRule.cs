using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to capture a random edge.
/// </summary>
public class RandomEdgeRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        var edges = board.EmptyEdges;
        if (edges.HasNoItems) return null;
        
        var random = new Random();
        var index = random.Next(edges.Length);
        
        return edges[index].Position;
    }
}