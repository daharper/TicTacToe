unit Core.Minimax;

interface

uses
  System.SysUtils,
  Core.Types,
  Core.Rules;

type
  /// <summary>Uses the minimax algorithm to try make an optimal move.</summary>
  TMinimax = record
  private
    /// <summary>
    /// Recursively evaluates the board state using the minimax algorithm and
    /// returns the score of the resulting position.
    /// </summary>
    class function Minimax(
      var aBoard: TBoardState;
      aCurrentPlayer, aMaximisingPlayer: TPieceKind;
      aDepth, aAlpha, aBeta: integer
    ): integer; static;

    /// <summary>
    /// Evaluates all legal moves for the maximising player and returns the
    /// highest attainable score.
    /// </summary>
    class function MaximiseScore(
      var aBoard: TBoardState;
      aCurrentPlayer, aMaximisingPlayer: TPieceKind;
      aDepth, aAlpha, aBeta: integer
    ): integer; static;

    /// <summary>
    /// Evaluates all legal moves for the minimising player and returns the
    /// lowest attainable score.
    /// </summary>
    class function MinimiseScore(
      var aBoard: TBoardState;
      aCurrentPlayer, aMaximisingPlayer: TPieceKind;
      aDepth, aAlpha, aBeta: integer
    ): integer; static;


    /// <summary>
    /// Returns the optimal move for the specified player.
    /// </summary>
    class function FindBestMove(
      const aBoard: TBoardState;
      aPieceKind: TPieceKind
    ): TBoardPosition; static;

  public
    /// <summary>
    /// Attempts to determine the optimal move for the specified player.
    /// </summary>
    class function TryFindBestMove(
      const aBoard: TBoardState;
      aPieceKind: TPieceKind;
      out aMove: TBoardPosition
    ): boolean; static;

  end;

implementation

uses
  System.Math;

{ TTicTacToeAI }

{----------------------------------------------------------------------------------------------------------------------}
class function TMinimax.MaximiseScore(
  var aBoard: TBoardState;
  aCurrentPlayer: TPieceKind;
  aMaximisingPlayer: TPieceKind;
  aDepth, aAlpha, aBeta: integer
): Integer;
var
  score: integer;
begin
  Result := Low(integer);

  for var position := bpTopLeft to bpBottomRight do
  begin
    if aBoard[position] <> pkNone then continue;

    aBoard[position] := aCurrentPlayer;

    try
      score := Minimax(
        aBoard,
        TRules.OpponentOf(aCurrentPlayer),
        aMaximisingPlayer,
        aDepth + 1,
        aAlpha,
        aBeta
      );
    finally
      aBoard[position] := pkNone;
    end;

    Result := Max(Result, score);

    aAlpha := Max(aAlpha, Result);

    if aBeta <= aAlpha then exit;
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TMinimax.MinimiseScore(
  var aBoard: TBoardState;
  aCurrentPlayer, aMaximisingPlayer: TPieceKind;
  aDepth, aAlpha, aBeta: integer
): integer;
var
  score: integer;
begin
  Result := High(integer);

  for var position := bpTopLeft to bpBottomRight do
  begin
    if aBoard[position] <> pkNone then continue;

    aBoard[position] := aCurrentPlayer;

    try
      score := Minimax(
        aBoard,
        TRules.OpponentOf(aCurrentPlayer),
        aMaximisingPlayer,
        aDepth + 1,
        aAlpha,
        aBeta
      );
    finally
      aBoard[position] := pkNone;
    end;

    Result := Min(Result, score);

    aBeta := Min(aBeta, Result);

    if aBeta <= aAlpha then exit;
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TMinimax.Minimax(
  var aBoard: TBoardState;
  aCurrentPlayer, aMaximisingPlayer: TPieceKind;
  aDepth, aAlpha, aBeta: Integer): Integer;
const
  CWinScore = 10;
begin
  var winner := TRules.WinnerOf(aBoard);

  if winner = aMaximisingPlayer then
    exit(CWinScore - aDepth);

  if winner = TRules.OpponentOf(aMaximisingPlayer) then
    exit(aDepth - CWinScore);

  if TRules.IsBoardFull(aBoard) then
    exit(0);

  Result :=
    if aCurrentPlayer = aMaximisingPlayer then
      MaximiseScore(aBoard, aCurrentPlayer, aMaximisingPlayer, aDepth, aAlpha, aBeta)
    else
      MinimiseScore(aBoard, aCurrentPlayer, aMaximisingPlayer, aDepth, aAlpha, aBeta);
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TMinimax.FindBestMove(const aBoard: TBoardState; aPieceKind: TPieceKind): TBoardPosition;
begin
  if not TryFindBestMove(aBoard, aPieceKind, Result) then
    raise EInvalidOpException.Create('There are no valid moves available');
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TMinimax.TryFindBestMove(
  const aBoard: TBoardState;
  aPieceKind: TPieceKind;
  out aMove: TBoardPosition): boolean;
var
  score: integer;
begin
  if aPieceKind = pkNone then
    raise EArgumentException.Create('aPieceKind must be X or O');

  Result := false;

  if TRules.WinnerOf(aBoard) <> pkNone then exit;

  if TRules.IsBoardFull(aBoard) then exit;

  var workingBoard := aBoard;
  var bestScore := Low(integer);

  for var position := bpTopLeft to bpBottomRight do
  begin
    if workingBoard[position] <> pkNone then continue;

    workingBoard[position] := aPieceKind;

    try
      score := Minimax(
        workingBoard,
        TRules.OpponentOf(aPieceKind),
        aPieceKind,
        0,
        Low(integer),
        High(integer)
      );
    finally
      workingBoard[position] := pkNone;
    end;

    if (not Result) or (score > bestScore) then
    begin
      bestScore := score;
      aMove := position;
      Result := true;
    end;
  end;
end;

end.
