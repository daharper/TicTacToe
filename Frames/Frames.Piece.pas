unit Frames.Piece;

interface

uses
  System.SysUtils,
  System.Types,
  System.UITypes,
  System.Classes,
  System.Variants,
  System.Math,
  FMX.Types,
  FMX.Controls,
  FMX.Forms,
  FMX.Graphics,
  FMX.Dialogs,
  FMX.Objects,
  Core.Types;

type
  TPiece = class(TFrame)
    pbPiece: TPaintBox;
    procedure FrameClick(Sender: TObject);
    procedure pbPiecePaint(Sender: TObject; Canvas: TCanvas);
  private
    fOnSelected: TNotifyEvent;

    { Private declarations }
    fKind: TPieceKind;
    fPosition: TBoardPosition;

    procedure SetKind(const AValue: TPieceKind);
  public
    { Public declarations }
    constructor Create(AOwner: TComponent); override;

    property OnSelected: TNotifyEvent read fOnSelected write fOnSelected;

    property Kind: TPieceKind read fKind write SetKind;
    property BoardPosition: TBoardPosition read fPosition write fPosition;

    class function New(AOwner: TComponent; const aPosition: TBoardPosition; const aKind: TPieceKind = pkNone): TPiece;
  end;

implementation

{$R *.fmx}

const
  PieceStrokeRatio = 0.10;
  MinPieceStrokeThickness = 1.0;
  PieceColorX = TAlphaColor($FFF36638);
  PieceColorO = TAlphaColors.White;

{----------------------------------------------------------------------------------------------------------------------}
constructor TPiece.Create(AOwner: TComponent);
begin
  inherited;

  if Assigned(pbPiece) then
    pbPiece.OnPaint := pbPiecePaint;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TPiece.FrameClick(Sender: TObject);
begin
  if not Assigned(fOnSelected) then exit;

  if Kind = pkNone then
    fOnSelected(Self);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TPiece.SetKind(const aValue: TPieceKind);
begin
  if fKind = aValue then exit;

  fKind := aValue;

  if Assigned(pbPiece) then
    pbPiece.Repaint;
end;

{----------------------------------------------------------------------------------------------------------------------}
class function TPiece.New(AOwner: TComponent; const aPosition: TBoardPosition; const aKind: TPieceKind): TPiece;
begin
  Result := TPiece.Create(AOwner);

  Result.fPosition := aPosition;
  Result.fKind     := aKind;
  Result.Name      := CPieceNames[aPosition];
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TPiece.pbPiecePaint(Sender: TObject; Canvas: TCanvas);
begin
  if fKind = TPieceKind.pkNone then Exit;

  var rect := pbPiece.LocalRect;
  var thickness: Single := Max(MinPieceStrokeThickness, Min(rect.Width, rect.Height) * PieceStrokeRatio);

  rect.Inflate(-thickness / 2, -thickness / 2);
  if rect.IsEmpty then Exit;

  Canvas.Stroke.Kind := TBrushKind.Solid;
  Canvas.Stroke.Dash := TStrokeDash.Solid;
  Canvas.Stroke.Cap := TStrokeCap.Round;
  Canvas.Stroke.Join := TStrokeJoin.Round;
  Canvas.Stroke.Thickness := thickness;

  case fKind of
    TPieceKind.pkX:
      begin
        Canvas.Stroke.Color := PieceColorX;

        Canvas.DrawLine(rect.TopLeft, rect.BottomRight, 1);
        Canvas.DrawLine(TPointF.Create(rect.Right, rect.Top), TPointF.Create(rect.Left, rect.Bottom), 1);
      end;
    TPieceKind.pkO:
      begin
        Canvas.Stroke.Color := PieceColorO;

        Canvas.DrawEllipse(rect, 1);
      end;
  end;
end;

end.
