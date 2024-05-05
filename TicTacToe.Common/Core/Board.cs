using TicTacToe.Common.Constants;

namespace TicTacToe.Common.Core;

/// <summary>
/// Represents a board of 9 squares, each square can be empty, a cross or a nought.
/// Provides utility methods for analyzing the board.
/// </summary>
public class Board
{
    public readonly Square[] Squares= 
    { 
        new (Position.TopLeft), 
        new (Position.TopCenter), 
        new (Position.TopRight), 
        new (Position.MiddleLeft), 
        new (Position.MiddleCenter), 
        new (Position.MiddleRight), 
        new (Position.BottomLeft), 
        new (Position.BottomCenter), 
        new (Position.BottomRight) 
    };
    
    #region values
    
    public Value this[Position position] => Squares[(int)position].Value;
    
    public Value this[int index] => Squares[index].Value;
    
    public Value this[int row, int col] => Squares[row * 3 + col].Value;
    
    #endregion

    #region move
    
    public void Move(Position position, Value value) => Squares[(int)position].Value = value;

    public bool IsWinningMove(Position position) => LinesFromPosition(position).Any(l => l.IsWin);
    
    public bool IsDraw => Squares.All(s => s.IsOccupied);
    
    #endregion
    
    #region position and indexing
    
    public Position PositionFrom(int row, int col) => (Position)(row * 3 + col);

    public static int RowIndexOf(Position position) 
        => position switch{
            Position.TopLeft or Position.TopCenter or Position.TopRight => 0,
            Position.MiddleLeft or Position.MiddleCenter or Position.MiddleRight => 1,
            Position.BottomLeft or Position.BottomCenter or Position.BottomRight => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Invalid position")
        };

    public static int ColumnIndexOf(Position position)
        => position switch
        {
            Position.TopLeft or Position.MiddleLeft or Position.BottomLeft => 0,
            Position.TopCenter or Position.MiddleCenter or Position.BottomCenter => 1,
            Position.TopRight or Position.MiddleRight or Position.BottomRight => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Invalid position")
        };
    
    #endregion
    
    #region squares
    
    public Square TopLeft => Squares[0];

    public Square TopCenter => Squares[1];

    public Square TopRight => Squares[2];

    public Square MiddleLeft => Squares[3];

    public Square MiddleCenter => Squares[4];

    public Square MiddleRight => Squares[5];

    public Square BottomLeft => Squares[6];

    public Square BottomCenter => Squares[7];

    public Square BottomRight => Squares[8];
    
    #endregion

    #region lines
    
    public Line TopRow => new(TopLeft, TopCenter, TopRight);
    
    public Line MiddleRow => new(MiddleLeft, MiddleCenter, MiddleRight);
    
    public Line BottomRow => new(BottomLeft, BottomCenter, BottomRight);
    
    public Line LeftColumn => new(TopLeft, MiddleLeft, BottomLeft);
    
    public Line MiddleColumn => new(TopCenter, MiddleCenter, BottomCenter);
    
    public Line RightColumn => new(TopRight, MiddleRight, BottomRight);
    
    public Line TopLeftToBottomRightDiagonal => new(TopLeft, MiddleCenter, BottomRight);
    
    public Line TopRightToBottomLeftDiagonal => new(TopRight, MiddleCenter, BottomLeft);

    public Line[] AllLines => new[]
    {
        TopRow, 
        MiddleRow, 
        BottomRow, 
        LeftColumn, 
        MiddleColumn, 
        RightColumn, 
        TopLeftToBottomRightDiagonal,
        TopRightToBottomLeftDiagonal
    };

    public Line[] Rows => new[] { TopRow, MiddleRow, BottomRow };

    public Line[] Columns => new[] { LeftColumn, MiddleColumn, RightColumn };

    public Line ColumnFrom(Position position) => Columns[ColumnIndexOf(position)];

    public Line RowFrom(Position position) => Rows[RowIndexOf(position)];

    public List<Line> DiagonalsFrom(Position position)
    {
        List<Line> diagonals = new();

        if (position == Position.MiddleCenter)
        {
            diagonals.Add(TopLeftToBottomRightDiagonal);
            diagonals.Add(TopRightToBottomLeftDiagonal);
        }
        else if (position == Position.TopLeft || position == Position.BottomRight)
        {
            diagonals.Add(TopLeftToBottomRightDiagonal);
        }
        else if (position == Position.TopRight || position == Position.BottomLeft)
        {
            diagonals.Add(TopRightToBottomLeftDiagonal);
        }
        
        return diagonals;
    }

    public List<Line> LinesFromPosition(Position position)
    {
        var lines = new List<Line>
        {
            RowFrom(position),
            ColumnFrom(position)
        };

        lines.AddRange(DiagonalsFrom(position));
        
        return lines;
    }
    
    #endregion
    
    #region sequences
    
    public Sequence<Square> Edges => new(TopCenter, MiddleLeft, MiddleRight, BottomCenter);
    
    public Sequence<Square> Corners => new(TopLeft, TopRight, BottomLeft, BottomRight);
    
    public Sequence<Square> EmptySquares => new(Squares.Where(s => s.IsEmpty).ToArray());

    public Sequence<Square> EmptyCorners => Corners.EmptySequence;

    public Sequence<Square> EmptyEdges => Edges.EmptySequence;
    
    /// <summary>
    /// Gets the opposite side of the specified side.
    /// </summary>
    public Value GetOtherSide(Value side) => side == Value.Cross ? Value.Nought : Value.Cross;

    /// <summary>
    /// Gets a sequence of winning positions for the specified side.
    /// </summary>
    public Sequence<Square> WinningSquares(Value side)
    {
        var squares = new List<Square>();
        
        foreach (var line in AllLines)
        {
            if (line.IsWinnable(side))
            {
                squares.Add(line.FirstEmpty!);
            }
        }

        return new Sequence<Square>(squares);
    }
    
    #endregion
}