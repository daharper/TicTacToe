using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;
using TicTacToe.Common.Rules;

namespace TicTacToe.Common.Players;

/// <summary>
/// Boris is a cautious player.
/// </summary>
public class Boris : ComputerPlayer
{
    public Boris(Value side) : base("Boris", side)
    {
        AddRule<FirstMoveRule>();
        AddRule<WinRule>();
        AddRule<BlockRule>();
        AddRule<CounterCornersRule>();
        AddRule<CenterRule>();
        AddRule<CreateForkRule>();
        AddRule<RandomRule>();

        Celebration = "Patience wins the day!";
    }
}
