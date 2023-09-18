using TicTacToe.Domain.Constants;
using TicTacToe.Domain.Core;

namespace TicTacToe.Domain.Rules
{
    public class MinimaxRule : Rule
    {
        public override Position? Execute(int moveCount, Board board, Value side)
        {
            var (row, col) = Analyzer.Execute(board, side);

            return board.PositionFrom(row, col);
        }
    }
}
