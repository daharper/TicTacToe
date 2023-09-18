using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Services;
using TicTacToe.Infrastructure.Utils;

using View = System.Console;

namespace TicTacToe.Console;

/// <summary>
/// Executes and manages the game.
/// </summary>
public static class GameRunner
{
    #region private

    private static readonly Dictionary<string, Statistic> Stats = new()
    {
        { "Tipsy", new Statistic("Tipsy") },
        { "Genghis", new Statistic("Genghis") },
        { "Boris", new Statistic("Boris") },
        { "Sima Yi", new Statistic("Sima Yi") }
    };

    private static readonly string[] Letters = { "A", "B", "C" };
    private static readonly string[] Numbers = { "1", "2", "3" };
    private static readonly string[] Pieces =  { " ", "X", "O" };
    
    private static readonly GameService Service = new();
    
    #endregion

    #region public methods
    
    public static void Execute()
    {
        View.Clear();
        DisplayStats();
        
        var game = Service.RandomComputerGame();

        DisplayPlayers(game);
        Thread.Sleep(2000);

        var canvas = CreateCanvas();
        DisplayCanvas(canvas);

        while (game.State == State.Running)
        {
            Thread.Sleep(500);
            
            var player = game.Turn;    
    
            var position = player.SelectMove(game.MoveCount, game.Board);
            game.Move(player, position);

            var piece = Pieces[(int)player.Side][0];
            var (row, column) = LocationFrom(position);
            
            var (x, y) = (2 + column * 2, 1 + row);
            canvas.Plot(piece, x, y);
            
            DisplayCanvas(canvas);
            DisplayMoves(game);
        }

        ProcessResults(game);
    }

    #endregion

    #region private methods
    
    private static (int col, int row) LocationFrom(Position position)
        => (Board.RowIndexOf(position), Board.ColumnIndexOf(position));
    
    private static Canvas CreateCanvas() => new(7, 4, "  1 2 3A _ _ _B _ _ _C _ _ _");

    private static void DisplayCanvas(Canvas canvas)
    {
        View.SetCursorPosition(0, 10);
        canvas.Display();
        View.WriteLine();
        View.WriteLine();
    }

    private static void DisplayStats()
    {
        View.WriteLine("  Name    G   W   D   L  Points  Win %");
        View.WriteLine("-------- --- --- --- --- ------ -------");

        Stats.Do(s => View.WriteLine(s.Value));

        View.WriteLine("---------------------------------------");
        View.WriteLine();
    }

    private static void DisplayPlayers(Game game)
        => View.WriteLine($"Now Playing => {game.Player1.Name} (x) vs {game.Player2.Name} (o)");

    private static void DisplayMoves(Game game)
    {
        View.WriteLine("------- Moves -------");
        View.WriteLine();
        
        game.Moves.DoWithIndex((m, i) =>
        {
            var turn = i % 2 == 0 ? game.Player1 : game.Player2;
            var piece = Pieces[(int)turn.Side];
            var (row, column) = LocationFrom(m);
            
            View.WriteLine("{0}. {1} to {2}{3}  ({4} selects {5})", 
                i + 1,
                piece,
                Letters[row], 
                Numbers[column],
                turn.Name,
                m);
        });
    }

    private static void ProcessResults(Game game)
    {
        Stats[game.Player1.Name].Games++;
        Stats[game.Player2.Name].Games++;

        var loser = game.Turn;

        if (game.State == State.Win)
        {
            if (loser == game.Player1)
            {
                Stats[game.Player1.Name].Losses++;
                Stats[game.Player2.Name].Wins++;
            }
            else
            {
                Stats[game.Player1.Name].Wins++;
                Stats[game.Player2.Name].Losses++;
            }
        }

        View.WriteLine();
        View.WriteLine(game.State == State.Win ? $"{loser.Name} lost!" : "It's a draw!");
    }

    #endregion
}