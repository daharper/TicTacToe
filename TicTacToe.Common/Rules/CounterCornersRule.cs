using TicTacToe.Common.Constants;
using TicTacToe.Common.Core;

namespace TicTacToe.Common.Rules;

/// <summary>
/// Tries to intelligently counter corner attacks.
/// </summary>
public class CounterCornersRule : Rule
{
    public override Position? Execute(int moveCount, Board board, Value side)
    {
        if (board.EmptyCorners.Length == 0) return null;

        var opp = board.GetOtherSide(side);
        
        var (myTop, myBottom) = GetCornerWeight(board, side);
        var (oppTop, oppBottom) = GetCornerWeight(board, opp);
        
        if (board.TopLeft.Value == opp && board.BottomCenter.Value == opp && board.BottomLeft.IsEmpty)
        {
            return Position.BottomLeft;
        }
        
        if (board.TopRight.Value == opp && board.BottomCenter.Value == opp && board.BottomRight.IsEmpty)
        {
            return Position.BottomRight;
        }
        
        if (board.BottomLeft.Value == opp && board.TopCenter.Value == opp && board.TopLeft.IsEmpty)
        {
            return Position.TopLeft;
        }
        
        if (board.BottomRight.Value == opp && board.TopCenter.Value == opp && board.TopRight.IsEmpty)
        {
            return Position.TopRight;
        }
        
        if (oppTop == 1 && oppBottom == 1 && myTop == 0 && myBottom == 0)
        {
            var position = new RandomEdgeRule().Execute(moveCount, board, side);
            if (position != null) return position;
        }

        if (oppTop > myTop)
        {
            if (board.TopLeft.IsEmpty) return board.TopLeft.Position;
            if (board.TopRight.IsEmpty) return board.TopRight.Position;
        }

        if (oppBottom > myBottom)
        {
            if (board.BottomLeft.IsEmpty) return board.BottomLeft.Position;
            if (board.BottomRight.IsEmpty) return board.BottomRight.Position;
        }
        
        if (oppTop == myTop && oppBottom == myBottom)
        {
            var (left, right) = GetColumnWeight(board, opp);

            if (left > right)
            {
                if (board.TopLeft.IsEmpty) return board.TopLeft.Position;
                if (board.BottomLeft.IsEmpty) return board.BottomLeft.Position;
            }

            if (right > left)
            {
                if (board.TopRight.IsEmpty) return board.TopRight.Position;
                if (board.BottomRight.IsEmpty) return board.BottomRight.Position;
            }

            return new RandomCornerRule().Execute(moveCount, board, side);
        }

        return new RandomEdgeRule().Execute(moveCount, board, side);
    }

    #region private methods

    private static (int top, int bottom) GetCornerWeight(Board board, Value side)
    {
        var top = board.TopRow.Filter(c => c.Value == side && c.IsCorner).Length;
        var bottom = board.BottomRow.Filter(c => c.Value == side && c.IsCorner).Length;

        return (top, bottom);
    }

    private static (int left, int right) GetColumnWeight(Board board, Value side)
    {
        var left = board.LeftColumn.Filter(c => c.Value == side).Length;
        var right = board.RightColumn.Filter(c => c.Value == side).Length;
        
        return (left, right);
    }

    #endregion
}