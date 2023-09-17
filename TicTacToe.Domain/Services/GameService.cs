using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Players;

namespace TicTacToe.Domain.Services;

/// <summary>
/// Service for creating and managing games.
/// </summary>
public class GameService
{
    #region public methods
    
    public Game RandomComputerGame()
    {
        var player1 = CreateRandomPlayer(Value.Cross);
        var player2 = CreateRandomPlayer(Value.Nought, player1.Name);
        
        return new Game(player1, player2);
    }
    
    public (Game game, Player player, Player computer) HumanVersus(string opponent)
    {
        Player human = null!;
        Player computer = null!;
        Game game = null!;
        
        if (new Random().Next(2) == 0)
        {
            human = new HumanPlayer(Value.Cross);
            computer = CreatePlayer(opponent, Value.Nought);
            game = new Game(human, computer); 
        }
        else
        {
            computer = CreatePlayer(opponent, Value.Cross);
            human = new HumanPlayer(Value.Nought);
            game = new Game(computer, human);
        }
        
        return (game, human, computer);
    }
    
    #endregion
    
    #region private methods

    private Player CreateRandomPlayer(Value side, string opponent = "")
    {
        Player player;
        
        do
        {
            player = new Random().Next(3) switch
            {
                0 => new Tipsy(side),
                1 => new Boris(side),
                2 => new Genghis(side)
            };
        } 
        while (player.Name == opponent);

        return player;
    }
    

    private Player CreatePlayer(string opponent, Value side)
        => opponent switch
        {
            "Tipsy" => new Tipsy(side),
            "Boris" => new Boris(side),
            "Genghis" => new Genghis(side),
            _ => throw new ArgumentException("Invalid opponent.", nameof(opponent)),
        };
    
    #endregion
}