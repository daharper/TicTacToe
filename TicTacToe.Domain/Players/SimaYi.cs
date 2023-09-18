using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Rules;

namespace TicTacToe.Domain.Players;

public class SimaYi : ComputerPlayer
{
    public SimaYi(Value side) : base("Sima Yi", side)
    {
        AddRule<MinimaxRule>();

        Celebration = "Ha ha ha ha ha ha!";
    }
}

