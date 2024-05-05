using TicTacToe.Common.Constants;
using TicTacToe.Common.Players;

namespace TicTacToe.Common.Core;

/// <summary>
/// A factory for creating Tic Tac Toe game instances.
/// </summary>
public class GameFactory
{
    #region public methods

    /// <summary>
    /// Returns a computer game initialized with two random computer players.
    /// </summary>
    public Game CreateComputerGame()
    {
        var player1 = CreateRandomPlayer(Value.Nought);
        var player2 = CreateRandomPlayer(Value.Cross, player1.Name);
        
        return new Game(player1, player2);
    }

    /// <summary>
    /// Returns a new game initialized for a user versus the specified computer opponent.
    /// </summary>
    public (Game game, Player player, Player computer) CreateUserVersus(string opponent)
    {
        Player human;
        Player computer;
        Game game;
        
        if (new Random().Next(2) == 0)
        {
            human = new HumanPlayer(Value.Nought);
            computer = CreatePlayer(opponent, Value.Cross);
            game = new Game(human, computer); 
        }
        else
        {
            human = new HumanPlayer(Value.Cross);
            computer = CreatePlayer(opponent, Value.Nought);
            game = new Game(computer, human);
        }
        
        return (game, human, computer);
    }
    
    #endregion
    
    #region private methods

    private static Player CreateRandomPlayer(Value side, string opponent = "")
    {
        Player player;
        
        do
        {
            player = new Random().Next(4) switch
            {
                0 => new Tipsy(side),
                1 => new Boris(side),
                2 => new Genghis(side),
                _ => new SimaYi(side),
            };
        } 
        while (player.Name == opponent);

        return player;
    }
    

    private static Player CreatePlayer(string opponent, Value side)
        => opponent switch
        {
            "Tipsy" => new Tipsy(side),
            "Boris" => new Boris(side),
            "Genghis" => new Genghis(side),
            "Sima Yi" => new SimaYi(side),
            _ => throw new ArgumentException("Invalid opponent.", nameof(opponent)),
        };
    
    #endregion
}