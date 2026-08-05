unit Core.Rules;

interface

uses
  Core.Types;

type
  /// <summary>
  /// Essential game rules.
  /// </summary>
  TRules = class
    /// <summary>
    /// Returns the opposing piece kind for the specified player.
    /// </summary>
    class function OpponentOf(aPieceKind: TPieceKind): TPieceKind; static;

    /// <summary>
    /// Determines whether either player has achieved a winning position on the board.
    /// Returns <c>pkNone</c> if there is no winner.
    /// </summary>
    class function WinnerOf(const aBoard: TBoardState): TPieceKind; static;

    /// <summary>
    /// Determines whether all board positions are occupied.
    /// </summary>
    class function IsBoardFull(const aBoard: TBoardState): boolean; static;

    /// <summary>
    /// Determines whether the position on the board is occupied.
    /// </summary>
    class function IsOccupied(const aBoard: TBoardState; aPosition: TBoardPosition): boolean; static;

    /// <summary>
    /// Determines whether the specified board position is empty.
    /// </summary>
    class function IsEmpty(const aBoard: TBoardState; aPosition: TBoardPosition): boolean; static;
  end;

const
  CWinningLines: array[0..7] of TWinningLine = (
    // Rows
    (bpTopLeft,     bpTopCenter,     bpTopRight),
    (bpMiddleLeft,  bpMiddleCenter,  bpMiddleRight),
    (bpBottomLeft,  bpBottomMiddle,  bpBottomRight),

    // Columns
    (bpTopLeft,     bpMiddleLeft,    bpBottomLeft),
    (bpTopCenter,   bpMiddleCenter,  bpBottomMiddle),
    (bpTopRight,    bpMiddleRight,   bpBottomRight),

    // Diagonals
    (bpTopLeft,     bpMiddleCenter,  bpBottomRight),
    (bpTopRight,    bpMiddleCenter,  bpBottomLeft)
  );

implementation

uses
  System.SysUtils;

{ TRules }

{----------------------------------------------------------------------------------------------------------------------}
class function TRules.IsBoardFull(const aBoard: TBoardState): Boolean;
begin
  for var position := bpTopLeft to bpBottomRight do
    if aBoard[Position] = pkNone then exit(false);

  Result := true;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TRules.IsEmpty(const aBoard: TBoardState; aPosition: TBoardPosition): boolean;
begin
  Result := aBoard[aPosition] = pkNone;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TRules.IsOccupied(const aBoard: TBoardState; aPosition: TBoardPosition): boolean;
begin
  Result := aBoard[aPosition] <> pkNone;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TRules.OpponentOf(aPieceKind: TPieceKind): TPieceKind;
begin
  case aPieceKind of
    pkX:
      Result := pkO;
    pkO:
      Result := pkX;
  else
    raise EArgumentException.Create('pkNone does not have an opponent');
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TRules.WinnerOf(const aBoard: TBoardState): TPieceKind;
begin
  Result := pkNone;

  for var i := Low(CWinningLines) to High(CWinningLines) do
  begin
    var line  := CWinningLines[i];
    var piece := aBoard[line[0]];

    if (Piece <> pkNone) and (aBoard[line[1]] = Piece) and (aBoard[line[2]] = Piece) then
      exit(Piece);
  end;
end;

end.
