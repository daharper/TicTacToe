using TicTacToe.Console;

var playing = true;

while (playing)
{
    try
    {
        GameRunner.Execute();
        Thread.Sleep(1000);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    }

    if (Console.KeyAvailable)
    {
        var info = Console.ReadKey(true);
        playing = info.KeyChar != 'x';
    }
}

