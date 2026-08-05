unit Core.Game;

interface

uses
  Core.Types,
  Core.Players;

type
  TGameState = (gsNone, gsActive, gsPlayerWon, gsAiWon, gsDraw);

  IGame = interface
    function MoveAi(out aMove: TBoardPosition): TGameState;
    function MovePlayer(const aMove: TBoardPosition): TGameState;
    function NextTurn: TPieceKind;
    function IsPlayerTurn: boolean;
    function IsAiTurn: boolean;
    function IsBoardFull: boolean;
    function IsWinningMove: boolean;
    function CalculateScore: Integer;
    function Board: TBoardState;
    function User: IPlayer;
    function Ai: IAiPlayer;
    function State: TGameState;
    procedure Stop;
  end;

  TGame = class(TInterfacedObject, IGame)
  private
    fMoves: integer;
    fTimer: integer;
    fAi: IAiPlayer;
    fUser: IPlayer;
    fOpponent: TOpponent;
    fBoard: TBoardState;
    fTurn: TPieceKind;
    fState: TGameState;

    function ResolveGameState: TGameState;

    constructor Create(const aTimer: integer; const aOpponent: TOpponent; const aOpponentPiece: TPieceKind);
  public
    function MoveAi(out aMove: TBoardPosition): TGameState;
    function MovePlayer(const aMove: TBoardPosition): TGameState;
    function Board: TBoardState;
    function NextTurn: TPieceKind;
    function IsPlayerTurn: boolean;
    function IsAiTurn: boolean;
    function IsBoardFull: boolean;
    function IsWinningMove: boolean;
    function CalculateScore: Integer;
    function User: IPlayer;
    function Ai: IAiPlayer;
    function State: TGameState;
    procedure Stop;

    class function Start(const aTimer: integer; const aOpponent: TOpponent): IGame;
  end;

implementation

uses
  Core.Rules,
  System.Math;

{ TGame }

{----------------------------------------------------------------------------------------------------------------------}
function TGame.Board: TBoardState;
begin
  Result := fBoard;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.Ai: IAiPlayer;
begin
  Result := fAi;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.User: IPlayer;
begin
  Result := fUser;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.CalculateScore: Integer;
const
  COpponentScores: array[TOpponent] of Integer = (100, 250, 500, 1000);
begin
  var moves := Max(1, fMoves div 2);

  Result := COpponentScores[fOpponent] + ((60 - fTimer) * 10) + Max(0, (9 - moves) * 50);
end;

{----------------------------------------------------------------------------------------------------------------------}
constructor TGame.Create(const aTimer: integer; const aOpponent: TOpponent; const aOpponentPiece: TPieceKind);
begin
  fTimer    := aTimer;
  fOpponent := aOpponent;
  fAi       := TAiFactory.New(aOpponentPiece, aOpponent);
  fUser     := TUser.New(TRules.OpponentOf(aOpponentPiece));
  fTurn     := pkX;
  fMoves    := 1;
  fState    := gsActive;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.State: TGameState;
begin
  Result := fState;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGame.Stop;
begin
  fTurn  := pkNone;
  fState := gsNone;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.IsBoardFull: boolean;
begin
  Result := TRules.IsBoardFull(fBoard);
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.IsPlayerTurn: boolean;
begin
  Result := fTurn = fUser.PieceKind;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.IsAiTurn: boolean;
begin
  Result := fTurn = fAi.PieceKind;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.IsWinningMove: boolean;
begin
  Result := TRules.WinnerOf(fBoard) = fTurn;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.MoveAi(out aMove: TBoardPosition): TGameState;
begin
  aMove := fAi.GetMove(fBoard);

  fBoard[aMove] := fAi.PieceKind;

  Result := ResolveGameState;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.MovePlayer(const aMove: TBoardPosition): TGameState;
begin
  fBoard[aMove] := fUser.PieceKind;

  Result := ResolveGameState;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TGame.Start(const aTimer: integer; const aOpponent: TOpponent): IGame;
begin
  var piece := if Random(2) = 0 then pkO else pkX;

  Result := TGame.Create(aTimer, aOpponent, piece);
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.NextTurn: TPieceKind;
begin
  if fTurn = pkO then
    fTurn := pkX
  else
    fTurn := pkO;

  Inc(fMoves);

  Result := fTurn;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TGame.ResolveGameState: TGameState;
begin
  var winner := TRules.WinnerOf(fBoard);

  if winner = fUser.PieceKind then
    fState := gsPlayerWon
  else if winner = fAi.PieceKind then
    fState := gsAiWon
  else if TRules.IsBoardFull(fBoard) then
    fState := gsDraw
  else if fMoves > 0 then
    fState := gsActive
  else
    fState := gsNone;

  Result := fState;
end;

end.
