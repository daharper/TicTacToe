using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Rules;

namespace TicTacToe.Domain.Players;

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
