using TicTacToe.Domain.Constants;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Minimax algorithm for Tic Tac Toe, adapted from code found on GeeksForGeeks:
/// https://www.geeksforgeeks.org/minimax-algorithm-in-game-theory-set-1-introduction/
/// </summary>
public static class Analyzer
{
    #region MyRegion

    private class Move
    {
        public int Row { get; set; }
        public int Col { get; set; }
    };

    private static char _player = 'x';
    private static char _opponent = 'o';

    #endregion

    /// <summary>
    /// Searches for the most efficient move using the minimax algorithm.
    /// </summary>
    public static (int row, int col) Execute(Board board, Value side)
    {
        if (side == Value.Cross)
        {
            _player = 'x';
            _opponent = 'o';
        }
        else
        {
            _player = 'o';
            _opponent = 'x';
        }

        var b = CreateBoard(board);

        var bestMove = FindBestMove(b);

        return (bestMove.Row, bestMove.Col);
    }

    private static char[,] CreateBoard(Board board)
    {
        var b = new char[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                b[i, j] = board[i, j] switch
                {
                    Value.Cross => 'x',
                    Value.Nought => 'o',
                    _ => '_'
                };
            }
        }

        return b;
    }

    private static bool IsMovesLeft(char[,] board)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (board[i, j] == '_') return true;
            }

        }

        return false;
    }

    private static int Evaluate(char[,] b)
    {
        for (var row = 0; row < 3; row++)
        {
            if (b[row, 0] == b[row, 1] && b[row, 1] == b[row, 2])
            {
                if (b[row, 0] == _player) return +10;
                if (b[row, 0] == _opponent) return -10;
            }
        }

        for (var col = 0; col < 3; col++)
        {
            if (b[0, col] == b[1, col] && b[1, col] == b[2, col])
            {
                if (b[0, col] == _player) return +10;
                if (b[0, col] == _opponent) return -10;
            }
        }

        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == _player) return +10;
            if (b[0, 0] == _opponent) return -10;
        }

        if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == _player) return +10;
            if (b[0, 2] == _opponent) return -10;
        }

        return 0;
    }

    private static int Minimax(char[,] board, int depth, bool isMax)
    {
        var score = Evaluate(board);

        if (score == 10) return score;
        if (score == -10) return score;

        if (IsMovesLeft(board) == false) return 0;

        if (isMax)
        {
            var best = -1000;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (board[i, j] != '_') continue;

                    board[i, j] = _player;

                    best = Math.Max(best, Minimax(board, depth + 1, !isMax));

                    board[i, j] = '_';
                }
            }

            return best;
        }
        else
        {
            var best = 1000;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (board[i, j] != '_') continue;

                    board[i, j] = _opponent;

                    best = Math.Min(best, Minimax(board, depth + 1, !isMax));

                    board[i, j] = '_';
                }
            }

            return best;
        }
    }

    private static Move FindBestMove(char[,] board)
    {
        var bestVal = -1000;

        var bestMove = new Move
        {
            Row = -1,
            Col = -1
        };

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (board[i, j] != '_') continue;

                board[i, j] = _player;

                var moveVal = Minimax(board, 0, false);

                board[i, j] = '_';

                if (moveVal > bestVal)
                {
                    bestMove.Row = i;
                    bestMove.Col = j;
                    bestVal = moveVal;
                }
            }
        }

        return bestMove;
    }
}
