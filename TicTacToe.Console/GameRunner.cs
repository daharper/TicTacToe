using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;
using TicTacToe.Domain.Players;
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
    
    private static readonly string[] Letters = { "A", "B", "C" };
    private static readonly string[] Numbers = { "1", "2", "3" };
    private static readonly string[] Pieces =  { " ", "X", "O" };
    
    private static readonly Player Tipsy = new Tipsy(Value.Cross);
    private static readonly Player Boris = new Boris(Value.Nought);

    private static readonly GameService Service = new();
    
    #endregion

    #region public methods
    
    public static (Player, State) Execute()
    {
        Player player = default!;
        
        View.Clear();
        
        var canvas = CreateCanvas();
        DisplayCanvas(canvas);

        var game = Service.RandomComputerGame();
        
        while (game.State == State.Running)
        {
            Thread.Sleep(1000);
            
            player = game.Turn;    
    
            var position = player.SelectMove(game.MoveCount, game.Board);
            game.Move(player, position);

            var piece = Pieces[(int)player.Side][0];
            var (row, column) = LocationFrom(position);
            
            var (x, y) = (2 + column * 2, 1 + row);
            canvas.Plot(piece, x, y);
            
            DisplayCanvas(canvas);
            DisplayMoves(game);
        }

        return (player, game.State);
    }
    
    #endregion

    #region private methods
    
    private static (int col, int row) LocationFrom(Position position)
        => (Board.RowIndexOf(position), Board.ColumnIndexOf(position));
    
    private static Canvas CreateCanvas()
        => new Canvas(7, 4, "  1 2 3A _ _ _B _ _ _C _ _ _");

    private static void DisplayCanvas(Canvas canvas)
    {
        View.SetCursorPosition(0, 0);
        canvas.Display();
        View.WriteLine();
    }
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
    
    #endregion
}