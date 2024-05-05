using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;
using TicTacToe.Common.Rules;

namespace TicTacToe.Common.Players;

/// <summary>
/// Genghis is an overly aggressive player.
/// </summary>
public class Genghis : ComputerPlayer
{
    public Genghis(Value side) : base("Genghis", side)
    {
        AddRule<CenterRule>();
        AddRule<WinRule>();
        AddRule<CreateForkRule>();
        AddRule<BlockRule>();
        AddRule<RandomRule>();
        
        Celebration = "I am the greatest!";
    }
}
