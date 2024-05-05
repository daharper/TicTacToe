using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;
using TicTacToe.Common.Rules;

namespace TicTacToe.Common.Players;

/// <summary>
/// Sima Yi is a boss level player.
/// </summary>
public class SimaYi : ComputerPlayer
{
    public SimaYi(Value side) : base("Sima Yi", side)
    {
        AddRule<MinimaxRule>();

        Celebration = "Ha ha ha ha ha ha!";
    }
}

