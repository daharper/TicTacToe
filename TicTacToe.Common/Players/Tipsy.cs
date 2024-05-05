using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;
using TicTacToe.Common.Rules;

namespace TicTacToe.Common.Players;

/// <summary>
/// Tipsy has had a bit to drink and makes random moves.
/// </summary>
public class Tipsy : ComputerPlayer
{
    public Tipsy(Value side) : base("Tipsy", side)
    {
        AddRule<RandomRule>();

        Celebration = "Wow! What happened?!";
    }
}