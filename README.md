# TicTacToe

Simple game to explore and experiment with Blazor WebAssembly.

![GameScreenShot](https://github.com/daharper/TicTacToe/assets/2164086/9313a024-9827-45bb-bcac-8be3ce78cbfc)

It is a player vs computer game, who goes first is randomly selected.

Three players (Tipsy, Genghis, Boris) use simple rules I created to drive the player.

Sima Yi uses the Minimax algorithm I adapted from a game site (see Analyzer in the code).

**Note**: If you play against Sima Yi there may be a one second delay betwen moves - at some point I'll
add a busy indicator. Boris seems to perform as well as Sima Yi and his moves are instant.

There is a console application included in the solution which randomly pits computer 
players against each other. Sima Yi and Boris are both unbeaten, but Sima Yi has the
better win percentage:

![Screenshot_20230918_033355](https://github.com/daharper/TicTacToe/assets/2164086/df55a205-8246-4100-9a28-e8d1c9298993)

This was a fun challenge to start learning about Blazor WebAssembly.
