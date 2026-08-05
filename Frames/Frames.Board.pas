unit Frames.Board;

interface

uses
  System.SysUtils,
  System.Generics.Collections,
  System.Types,
  System.UITypes,
  System.Classes,
  System.Variants,
  FMX.Types,
  FMX.Controls,
  FMX.Forms,
  FMX.Graphics,
  FMX.Dialogs,
  FMX.Layouts,
  Core.Types,
  Frames.Piece;

type
  TPieceEvent = procedure(const aPiece: TPiece) of object;

  TBoard = class(TFrame)
    CellGrid: TGridPanelLayout;
  private
    fPieceMap: TDictionary<TBoardPosition, TPiece>;
    fOnSelection: TPieceEvent;

    procedure OnPieceSelected(Sender: TObject);
    procedure AddPiece(const aPosition: TBoardPosition);
  public
    property OnSelection: TPieceEvent read fOnSelection write fOnSelection;

    procedure AfterConstruction; override;
    procedure BeforeDestruction; override;
    procedure Initialize(const aState: TBoardState);
    procedure UpdatePiece(const aPosition: TBoardPosition; const aKind: TPieceKind);
  end;

implementation

{$R *.fmx}

{ TBoard }

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.AfterConstruction;
begin
  inherited;

  fPieceMap := TDictionary<TBoardPosition, TPiece>.Create;

  for var position := bpTopLeft to bpBottomRight do
    AddPiece(position);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.BeforeDestruction;
begin
  fPieceMap.Free;

  inherited;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.AddPiece(const aPosition: TBoardPosition);
begin
  var piece := TPiece.New(Self, aPosition);

  piece.Parent      := CellGrid;
  piece.Align       := TAlignLayout.Client;
  piece.OnSelected  := OnPieceSelected;

  fPieceMap.Add(aPosition, piece);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.Initialize(const aState: TBoardState);
begin
  for var position := bpTopLeft to bpBottomRight do
    fPieceMap[position].Kind := aState[position];
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.OnPieceSelected(Sender: TObject);
begin
  if not Assigned(fOnSelection) then exit;

  var piece := TPiece(Sender);

  fOnSelection(piece);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoard.UpdatePiece(const aPosition: TBoardPosition; const aKind: TPieceKind);
begin
  fPieceMap[aPosition].Kind := aKind;
end;

end.
