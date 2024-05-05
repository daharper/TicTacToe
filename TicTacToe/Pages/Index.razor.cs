using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;
using TicTacToe.Data.Constants;

namespace TicTacToe.Pages;

public partial class Index
{
    #region private
    
    private Timer _timer = null!;
    private string _clock = "";
    private int _seconds = 0;
    
    #endregion
    
    #region properties
    
    private Player? HumanPlayer { get; set; }
    private Player? ComputerPlayer { get; set; }
    private Game? Game { get; set; }
    private GameResult Result { get; set; } = GameResult.None;
    private string Message { get; set; } = "Game in progress";
    private string Opponent { get; set; } = "Tipsy";

    #endregion
    
    /// <summary>
    /// Initializes the game, creates the timer.
    /// </summary>
    protected override void OnInitialized()
    {
        (Game, HumanPlayer, ComputerPlayer) = Factory.CreateUserVersus(Opponent);
        ResetClock();
        CreateTimer();
        ComputerMove();
    }
    
    /// <summary>
    /// Starts a new game, resets the timer.
    /// </summary>
    private void StartGame()
    {
        Message = "Game in progress";
        Result = GameResult.None;

        (Game, HumanPlayer, ComputerPlayer) = Factory.CreateUserVersus(Opponent);
        ComputerMove();
        ResetClock();
        StateHasChanged();
    }

    /// <summary>
    /// Performs a move by the human player.
    /// </summary>
    /// <param name="square">The square where the move is made.</param>
    private void PlayerMove(Square square)
    {
        if (Game!.State != State.Running) return;
        if (Game.Turn != HumanPlayer) return;
        if (square.IsOccupied) return;
        
        UpdateGame(HumanPlayer, square.Position);

        ComputerMove();
        StateHasChanged();
    }

    /// <summary>
    /// Executes a move for the computer player.
    /// </summary>
    private void ComputerMove()
    {
        if (Game!.State != State.Running) return;
        if (Game.Turn != ComputerPlayer) return;

        var position = ComputerPlayer.SelectMove(Game.MoveCount, Game.Board);
        UpdateGame(ComputerPlayer, position);
    }

    /// <summary>
    /// Updates the game state after a player's move.
    /// </summary>
    /// <param name="player">The player who made the move.</param>
    /// <param name="position">The position where the move was made.</param>
    private void UpdateGame(Player player, Position position)
    {
        Game!.Move(player, position);

        switch (Game.State)
        {
            case State.Win:
                Result = player == HumanPlayer ? GameResult.PlayerWon : GameResult.ComputerWon;
                Message = $"{player.Name} won, saying '{player.Celebration}'";
                break;
            case State.Draw:
                Result = GameResult.Draw;
                Message = "Draw!";
                break;
        }
    }

    /// <summary>
    /// Creates a timer to track the game duration.
    /// </summary>
    private void CreateTimer()
    {
        _timer = new Timer(
            _ =>
            {
                if (Game is null || Game.State != State.Running) return;

                ++_seconds;
                _clock = TimeSpan.FromSeconds(_seconds).ToString(@"mm\:ss");

                InvokeAsync(StateHasChanged);
            },
            new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Resets the clock to its initial state of "00:00".
    /// </summary>
    private void ResetClock()
    {
        _seconds = 0;
        _clock = "00:00";
    }

    /// <summary>
    /// Sets the player's opponent for the game. There are four possible opponents:
    ///     - Tipsy
    ///     - Ghengis
    ///     - Boris
    ///     - Sima Yi
    /// </summary>
    /// <param name="name">The name of the opponent.</param>
    private void SetOpponent(string name)
    {
        Opponent = name;
        StartGame();
    }
}