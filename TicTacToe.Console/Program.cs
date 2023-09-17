using TicTacToe.Console;
using TicTacToe.Domain.Constants;

var playing = true;

while (playing)
{
    try
    {
        var (player, state) = GameRunner.Execute();

        Console.WriteLine();
        Console.WriteLine(state switch
        {
            State.Win => $"{player.Name} wins!",
            State.Draw => "It's a draw!",
            _ => "Game over."
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine();
    Console.WriteLine("Press (R) to replay or any other key to exit.");

    playing = Console.ReadKey().Key == ConsoleKey.R;
}

