unit Frames.BoardBackground;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  FMX.Types, FMX.Controls, FMX.Forms, FMX.Graphics, FMX.Dialogs, FMX.Objects;

type
  TBoardBackground = class(TFrame)
    pbGrid: TPaintBox;
    procedure pbGridPaint(Sender: TObject; Canvas: TCanvas);
  private
    { Private declarations }
  public
    { Public declarations }
    constructor Create(AOwner: TComponent); override;
    function CellRect(const aCol, aRow: Integer): TRectF;
  end;

implementation

{$R *.fmx}

const
  GridLineThickness = 8;
  GridLineColor = TAlphaColor($FF36424A);

{----------------------------------------------------------------------------------------------------------------------}
constructor TBoardBackground.Create(AOwner: TComponent);
begin
  inherited;

  if Assigned(pbGrid) then
    pbGrid.OnPaint := pbGridPaint;
end;

{----------------------------------------------------------------------------------------------------------------------}
function TBoardBackground.CellRect(const aCol, aRow: Integer): TRectF;
begin
  var cellWidth := pbGrid.Width / 3;
  var cellHeight := pbGrid.Height / 3;

  Result := TRectF.Create(ACol * cellWidth, aRow * cellHeight, (ACol + 1) * cellWidth, (aRow + 1) * cellHeight);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TBoardBackground.pbGridPaint(Sender: TObject; Canvas: TCanvas);
begin
  var rect := pbGrid.LocalRect;

  Canvas.Stroke.Kind := TBrushKind.Solid;
  Canvas.Stroke.Dash := TStrokeDash.Solid;
  Canvas.Stroke.Cap := TStrokeCap.Flat;
  Canvas.Stroke.Color := GridLineColor;
  Canvas.Stroke.Thickness := GridLineThickness;

  for var i := 1 to 2 do
  begin
    var x := rect.Left + (rect.Width / 3) * i;
    Canvas.DrawLine(TPointF.Create(x, rect.Top), TPointF.Create(x, rect.Bottom), 1);

    var y := rect.Top + (rect.Height / 3) * i;
    Canvas.DrawLine(TPointF.Create(rect.Left, y), TPointF.Create(rect.Right, y), 1);
  end;
end;

end.
