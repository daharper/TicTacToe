using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Represents a human player.
/// </summary>
public class HumanPlayer : Player
{
    public HumanPlayer(Value side, string name = "You") : base(name, side) { }

    public override Position SelectMove(int moveCount, Board board)
        => throw new InvalidOperationException(
            "Human players must select their own moves.");
}
