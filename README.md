# TicTacToe

Simple game to explore and experiment with Blazor WebAssembly.

![GameScreenShot](https://github.com/daharper/TicTacToe/assets/2164086/9313a024-9827-45bb-bcac-8be3ce78cbfc)

It is a player vs computer game, who goes first is randomly selected.

Three players (Tipsy, Genghis, Boris) use simple rules I created to drive the player.

Sima Yi uses the Minimax algorithm I adapted from a game site, he is unbeatable.

You select a player by clicking on the button with their name, located below the board.

**Note**: If you play against Sima Yi there may be a one second delay betwen moves - at some point I'll
add a busy indicator. Boris seems to perform almost as well as Sima Yi without any delay.

There is a console application included in the solution which randomly pits computer 
players against each other. 
It keeps statistics and continues until the user hits x.
Sima Yi and Boris are both unbeaten:

![ConsoleScreenShot](https://github.com/daharper/TicTacToe/assets/2164086/a6aa7608-5e5f-4080-94cb-684c348c2b4d)

The results are fairly consistant over many runs.

This was a fun challenge to start learning about Blazor WebAssembly.
