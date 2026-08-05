unit Core.Players;

interface

uses
  System.Generics.Collections,
  Core.Tactics,
  Core.Types;

type
  IPlayer = interface
    function GetPieceKind: TPieceKind;
    property PieceKind: TPieceKind read GetPieceKind;
  end;

  IAiPlayer = interface(IPlayer)
    function GetMove(const aBoardState: TBoardState): TBoardPosition;
  end;

  TUser = class(TInterfacedObject, IPlayer)
  private
    fPieceKind: TPieceKind;
    constructor Create(const aKind: TPieceKind);
  public
    function GetPieceKind: TPieceKind;
    property PieceKind: TPieceKind read GetPieceKind;

    class function New(const aPiece: TPieceKind): IPlayer;
  end;

  TAiPlayer = class(TInterfacedObject, IAiPlayer)
  private
    fPieceKind: TPieceKind;
    fTactics: TList<ITactic>;
  protected
    procedure Add(const aTactic: ITactic);
  public
    function GetPieceKind: TPieceKind;
    function GetMove(const aBoard: TBoardState): TBoardPosition; virtual;

    property PieceKind: TPieceKind read GetPieceKind;

    constructor Create(const aKind: TPieceKind);
    destructor Destroy; override;
  end;

  TTipsy = class(TAiPlayer)
  private
    class var fRandomTactic: ITactic;
    class var fWinTactic: ITactic;
    class var fBlockTactic: ITactic;
    class var fForkTactic: ITactic;
    class var fBlockForkTactic: ITactic;
    class var fCenterTactic: ITactic;
    class var fOppositeCornerTactic: ITactic;
    class var fCornerTactic: ITactic;
    class var fRandomCornerTactic: ITactic;
    class var fSideTactic: ITactic;
    class var fMinimaxTactic: ITactic;
  public
    function GetMove(const aBoard: TBoardState): TBoardPosition; override;
    constructor Create(const aKind: TPieceKind);
    class constructor Create;
  end;

  TGenghis = class(TAiPlayer)
  public
    constructor Create(const aKind: TPieceKind);
  end;

  TBoris = class(TAiPlayer)
  public
    constructor Create(const aKind: TPieceKind);
  end;

  TSimaYi = class(TAiPlayer)
  public
    constructor Create(const aKind: TPieceKind);
  end;

  TAiFactory = class
  public
    class function New(const aPieceKind: TPieceKind; const aPlayer: TOpponent): IAiPlayer;
  end;

implementation

{ TAiPlayer }

{----------------------------------------------------------------------------------------------------------------------}
procedure TAiPlayer.Add(const aTactic: ITactic);
begin
  fTactics.Add(aTactic);
end;

{----------------------------------------------------------------------------------------------------------------------}
function TAiPlayer.GetMove(const aBoard: TBoardState): TBoardPosition;
begin
  for var tactic in fTactics do
    if tactic.TryGetMove(aBoard, fPieceKind, Result) then exit;
end;

{----------------------------------------------------------------------------------------------------------------------}
constructor TAiPlayer.Create(const aKind: TPieceKind);
begin
  fPieceKind := aKind;

  fTactics := TList<ITactic>.Create;
end;

{----------------------------------------------------------------------------------------------------------------------}
destructor TAiPlayer.Destroy;
begin
  fTactics.Free;

  inherited;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TAiPlayer.GetPieceKind: TPieceKind;
begin
  Result := fPieceKind;
end;

{ TAiFactory }

{----------------------------------------------------------------------------------------------------------------------}
class function TAiFactory.New(const aPieceKind: TPieceKind; const aPlayer: TOpponent): IAiPlayer;
begin
  case aPlayer of
    opTipsy:   Result := TTipsy.Create(aPieceKind);
    opGenghis: Result := TGenghis.Create(aPieceKind);
    opBoris:   Result := TBoris.Create(aPieceKind);
    opSimaYi:  Result := TSimaYi.Create(aPieceKind);
  end;
end;

{ TTipsy }

{----------------------------------------------------------------------------------------------------------------------}
function TTipsy.GetMove(const aBoard: TBoardState): TBoardPosition;
begin
  Result := bpNone;

  var tactic: ITactic;

  case Random(20) of
    0..3:
      tactic := fRandomTactic;
    4:
      tactic := fCenterTactic;
    5:
      tactic := fWinTactic;
    6:
      tactic := fBlockTactic;
    7..13:
      tactic := fMinimaxTactic;
    14:
      tactic := fForkTactic;
    15:
      tactic := fBlockForkTactic;
    16:
      tactic := fSideTactic;
    17:
      tactic := fOppositeCornerTactic;
    18:
      tactic := fCornerTactic;
    else
      tactic := fRandomCornerTactic;
  end;

  if not tactic.TryGetMove(aBoard, PieceKind, Result) then
    fRandomTactic.TryGetMove(aBoard, PieceKind, Result);
end;

{----------------------------------------------------------------------------------------------------------------------}
constructor TTipsy.Create(const aKind: TPieceKind);
begin
  inherited Create(aKind);
end;

{----------------------------------------------------------------------------------------------------------------------}
class constructor TTipsy.Create;
begin
  fRandomTactic         := TRandomTactic.Create;
  fWinTactic            := TWinTactic.Create;
  fBlockTactic          := TBlockTactic.Create;
  fForkTactic           := TForkTactic.Create;
  fBlockForkTactic      := TBlockForkTactic.Create;
  fCenterTactic         := TCenterTactic.Create;
  fOppositeCornerTactic := TOppositeCornerTactic.Create;
  fCornerTactic         := TCornerTactic.Create;
  fRandomCornerTactic   := TRandomCornerTactic.Create;
  fSideTactic           := TSideTactic.Create;
  fMinimaxTactic        := TMinimaxTactic.Create;
end;

{ TGenghis }

{----------------------------------------------------------------------------------------------------------------------}
constructor TGenghis.Create(const aKind: TPieceKind);
begin
  inherited Create(aKind);

  Add(TWinTactic.Create);
  Add(TForkTactic.Create);
  Add(TCenterTactic.Create);
  Add(TRandomCornerTactic.Create);
  Add(TRandomTactic.Create);
end;

{ TBoris }

{----------------------------------------------------------------------------------------------------------------------}
constructor TBoris.Create(const aKind: TPieceKind);
begin
  inherited Create(aKind);

  Add(TWinTactic.Create);
  Add(TBlockTactic.Create);
  Add(TForkTactic.Create);
  Add(TBlockForkTactic.Create);
  Add(TCenterTactic.Create);
  Add(TOppositeCornerTactic.Create);
  Add(TCornerTactic.Create);
  Add(TSideTactic.Create);
end;

{ TSimaYi }

{----------------------------------------------------------------------------------------------------------------------}
constructor TSimaYi.Create(const aKind: TPieceKind);
begin
  inherited Create(aKind);

  Add(TMinimaxTactic.Create);
end;


{ TUser }

{----------------------------------------------------------------------------------------------------------------------}
function TUser.GetPieceKind: TPieceKind;
begin
  Result := fPieceKind;
end;

{----------------------------------------------------------------------------------------------------------------------}
constructor TUser.Create(const aKind: TPieceKind);
begin
  fPieceKind := aKind;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TUser.New(const aPiece: TPieceKind): IPlayer;
begin
  Result := TUser.Create(aPiece);
end;

end.
