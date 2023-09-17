using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Rules;

namespace TicTacToe.Domain.Players;

/// <summary>
/// Tipsy has had a bit to drink and makes random moves.
/// </summary>
public class Tipsy : ComputerPlayer
{
    public Tipsy(Value side) : base("Tipsy", side)
    {
        AddRule(new RandomRule());

        Celebration = "Wow! What happened?!";
    }
}