global using SquareSequence = TicTacToe.Infrastructure.Utils.Sequence<TicTacToe.Domain.Core.Square>;

using TicTacToe.Domain.Constants;
using TicTacToe.Infrastructure.Integrity;

namespace TicTacToe.Domain.Core;

/// <summary>
/// Represents a board of 9 squares, each square can be empty, a cross or a nought.
/// </summary>
public class Board
{
    public Square[] Squares;

    public Board() => Squares = Clear();

    public Square[] Clear()
        => new []{ 
            new Square(Position.TopLeft), 
            new Square(Position.TopCenter), 
            new Square(Position.TopRight), 
            new Square(Position.MiddleLeft), 
            new Square(Position.MiddleCenter), 
            new Square(Position.MiddleRight), 
            new Square(Position.BottomLeft), 
            new Square(Position.BottomCenter), 
            new Square(Position.BottomRight) 
        };
    
    #region values
    
    public Value this[Position position] 
        => Squares[(int)position].Value;
    
    public Value this[int index] 
        => Squares[index].Value;
    
    public Value this[int row, int col] 
        => Squares[row * 3 + col].Value;
    
    #endregion

    #region move
        
    public bool IsEmpty => Squares.All(s => s.IsEmpty);
    
    public bool IsAvailable(Position position) 
        => Squares[(int)position].IsEmpty;
    
    public void Move(Position position, Value value)
    {
        Bob.Assumes.IsTrue(Squares[(int)position].IsEmpty, "Position is already taken.");
        Squares[(int)position].Value = value;
    }

    public bool IsWinningMove(Position position) 
        => LinesFromPosition(position).Any(l => l.IsWin);
    
    public bool IsDraw 
        => Squares.All(s => s.IsOccupied);
    
    #endregion
    
    #region position and indexing
    
    public int IndexOf(Position position) => (int)position;
    
    public Position PositionOf(int index) => (Position)index;

    public string LabelFor(Position position) => position switch
    {
        Position.TopLeft => "A1",
        Position.TopCenter => "A2",
        Position.TopRight => "A3",
        Position.MiddleLeft => "B1",
        Position.MiddleCenter => "B2",
        Position.MiddleRight => "B3",
        Position.BottomLeft => "C1",
        Position.BottomCenter => "C2",
        Position.BottomRight => "C3",
        _ => throw new ArgumentOutOfRangeException(nameof(Position), position, null)
    };
    
    public static bool IsEdge(Position position) 
        => position is 
            Position.TopCenter or 
            Position.MiddleLeft or 
            Position.MiddleRight or 
            Position.BottomCenter;
    
    public static bool IsCorner(Position position) 
        => position is 
            Position.TopLeft or 
            Position.TopRight or
            Position.BottomLeft or 
            Position.BottomRight;
    
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
    
    public SquareSequence GetPositions(Value side)
        => new SquareSequence(Squares.Where(s => s.Value == side));

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
    
    public Square Center => MiddleCenter;
    
    public Square Square(int index) => Squares[index];
    
    public Square Square(int row, int col) => Squares[row * 3 + col];
    
    public Square Square(Position position) => Squares[(int)position];
    
    public Square SquareAt(Position position) => Squares[(int)position];
    
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
    
    public SquareSequence Edges => new(TopCenter, MiddleLeft, MiddleRight, BottomCenter);
    
    public SquareSequence Corners => new(TopLeft, TopRight, BottomLeft, BottomRight);
    
    public SquareSequence EmptySquares => new(Squares.Where(s => s.IsEmpty).ToArray());

    public SquareSequence EmptyCorners => Corners.EmptySequence;

    public SquareSequence EmptyEdges => Edges.EmptySequence;
    
    /// <summary>
    /// Gets a sequence of winning positions for the specified side.
    /// </summary>
    public SquareSequence WinningSquares(Value side)
    {
        var squares = new List<Square>();
        
        foreach (var line in AllLines)
        {
            if (line.IsWinnable(side))
            {
                squares.Add(line.FirstEmpty!);
            }
        }

        return new SquareSequence(squares);
    }

    /// <summary>
    /// Gets the opposite side of the specified side.
    /// </summary>
    public Value GetOtherSide(Value side)
        => side == Value.Cross ? Value.Nought : Value.Cross;

    #endregion
}