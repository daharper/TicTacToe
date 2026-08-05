unit Core.Tactics;

interface

uses
  Core.Types,
  Core.Minimax;

type
  ITactic = interface
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): boolean;
  end;

  TRandomTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): boolean;
  end;

  TCenterTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): boolean;
  end;

  TBlockTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TWinTactic = class(TInterfacedObject, ITactic)
  private
    class var fBlockTactic: ITactic;
  public
    class constructor Create;
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TForkTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
    class function CountWinningMoves(const aBoard: TBoardState; const aKind: TPieceKind): Integer; static;
  end;

  TBlockForkTactic = class(TInterfacedObject, ITactic)
  private
    class var fForkTactic: ITactic;
    class function HasFork(const aBoard: TBoardState; const aKind: TPieceKind): Boolean; static;
  public
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
    class constructor Create;
  end;

  TCornerTactic = class(TInterfacedObject, ITactic)
  private
    function GetAvailableCorners(const aBoard: TBoardState): TBoardPositions;
  public
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TRandomCornerTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TSideTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TOppositeCornerTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

  TMiniMaxTactic = class(TInterfacedObject, ITactic)
    function TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
  end;

implementation

uses
  Core.Rules;

{ TRandomTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TRandomTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): boolean;
begin
  repeat
    aMove := TBoardPosition(Random(CPositionCount));
  until TRules.IsEmpty(aBoard, aMove);

  Result := true;
end;

{ TCenterTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TCenterTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): boolean;
begin
  aMove := bpNone;

  Result := TRules.IsEmpty(aBoard, bpMiddleCenter);

  if Result then
    aMove := bpMiddleCenter;
end;

{ TBlockTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TBlockTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  var opponentKind := TRules.OpponentOf(aKind);

  for var position := bpTopLeft to bpBottomRight do
  begin
    if TRules.IsOccupied(aBoard, position) then continue;

    var testBoard := aBoard;

    testBoard[position] := opponentKind;

    if TRules.WinnerOf(testBoard) = opponentKind then
    begin
      aMove := position;
      exit(true);
    end;
  end;

  Result := false;
end;

{ TWinTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TWinTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  Result := fBlockTactic.TryGetMove(aBoard, TRules.OpponentOf(aKind), aMove);
end;

{----------------------------------------------------------------------------------------------------------------------}
class constructor TWinTactic.Create;
begin
  fBlockTactic := TBlockTactic.Create;
end;

{ TMiniMaxTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TMiniMaxTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  Result := TMiniMax.TryFindBestMove(aBoard, aKind, aMove);
end;

{ TCornerTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TCornerTactic.GetAvailableCorners(const aBoard: TBoardState): TBoardPositions;
begin
  SetLength(Result, Length(CCornerPositions));

  var count := 0;

  for var corner in CCornerPositions do
    if TRules.IsEmpty(aBoard, corner) then
    begin
      Result[count] := corner;
      Inc(count);
    end;

  SetLength(Result, count);
end;

{----------------------------------------------------------------------------------------------------------------------}
function TCornerTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  var availableCorners := GetAvailableCorners(aBoard);

  Result := Length(availableCorners) > 0;

  if Result then
    aMove := availableCorners[0];
end;

{ TRandomCornerTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TRandomCornerTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  var availableCorners: array[0..3] of TBoardPosition;
  var count := 0;

  for var corner in CCornerPositions do
    if TRules.IsEmpty(aBoard, corner) then
    begin
      availableCorners[count] := corner;
      Inc(count);
    end;

  if count = 0 then
    exit(false);

  aMove := availableCorners[Random(count)];
  Result := true;
end;


{ TForkTactic }

{----------------------------------------------------------------------------------------------------------------------}
class function TForkTactic.CountWinningMoves(const aBoard: TBoardState; const aKind: TPieceKind): Integer;
begin
  Result := 0;

  for var position := bpTopLeft to bpBottomRight do
  begin
    if TRules.IsOccupied(aBoard, position) then continue;

    var testBoard := aBoard;
    testBoard[position] := aKind;

    if TRules.WinnerOf(testBoard) = aKind then
      Inc(Result);
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TForkTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  for var position := bpTopLeft to bpBottomRight do
  begin
    if TRules.IsOccupied(aBoard, position) then continue;

    var testBoard := aBoard;
    testBoard[position] := aKind;

    if CountWinningMoves(testBoard, aKind) >= 2 then
    begin
      aMove := position;
      exit(True);
    end;
  end;

  Result := False;
end;

{ TBlockForkTactic }

{----------------------------------------------------------------------------------------------------------------------}
class function TBlockForkTactic.HasFork(const aBoard: TBoardState; const aKind: TPieceKind): Boolean;
begin
  var position: TBoardPosition;
  Result := fForkTactic.TryGetMove(aBoard, aKind, position);
end;

{----------------------------------------------------------------------------------------------------------------------}
function TBlockForkTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  var opponentKind := TRules.OpponentOf(aKind);

  if not HasFork(aBoard, opponentKind) then exit(false);

  for var position := bpTopLeft to bpBottomRight do
  begin
    if TRules.IsOccupied(aBoard, position) then continue;

    var testBoard := aBoard;
    testBoard[position] := aKind;

    if not HasFork(testBoard, opponentKind) then
    begin
      aMove := position;
      exit(true);
    end;
  end;

  Result := false;
end;

{----------------------------------------------------------------------------------------------------------------------}
class constructor TBlockForkTactic.Create;
begin
  fForkTactic := TForkTactic.Create;
end;

{ TSideTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TSideTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  for var side in CSidePositions do
    if TRules.IsEmpty(aBoard, side) then
    begin
      aMove := side;
      exit(true);
    end;

  Result := false;
end;

{ TOppositeCornerTactic }

{----------------------------------------------------------------------------------------------------------------------}
function TOppositeCornerTactic.TryGetMove(const aBoard: TBoardState; const aKind: TPieceKind; out aMove: TBoardPosition): Boolean;
begin
  aMove := bpNone;

  var opponent := TRules.OpponentOf(aKind);

  for var corner in CCornerPositions do
  begin
    if aBoard[corner] <> opponent then continue;

    var opposite := COppositeCorners[corner];

    if TRules.IsEmpty(aBoard, opposite) then
    begin
      aMove := opposite;
      exit(true);
    end;
  end;

  Result := False;
end;

end.
