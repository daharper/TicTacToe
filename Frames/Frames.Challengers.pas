unit Frames.Challengers;

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
  FMX.StdCtrls,
  FMX.Objects,
  FMX.Controls.Presentation,
  Core.Types;

type
  TChallengers = class(TFrame)
    OpponentGroup: TRectangle;
    CaptionLabel: TLabel;
    OptionLayout: TLayout;
  private
    fOpponent: TOpponent;
    fGroupName: string;
    fButtonMap: TDictionary<TOpponent, TRadioButton>;
    fOnOpponentChange: TNotifyEvent;

    procedure AddRadioButton(const aOpponent: TOpponent);
    procedure DoOpponentChange;
    procedure RadioButtonChange(Sender: TObject);
    procedure SetOpponent(const aValue: TOpponent);
  public
    procedure AfterConstruction; override;
    procedure BeforeDestruction; override;

    property Opponent: TOpponent read fOpponent write SetOpponent;
    property OnOpponentChange: TNotifyEvent read fOnOpponentChange write fOnOpponentChange;
  end;

implementation

{$R *.fmx}

const
  CDefaultOpponent = opTipsy;
  CButtonSpacing   = 4;

  CButtonWidths: array [TOpponent] of integer = (70, 86, 70, 86);

{ TChallengers }

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.AfterConstruction;
begin
  inherited;

  fButtonMap := TDictionary<TOpponent, TRadioButton>.Create;
  fGroupName := 'Challengers' + IntToHex(NativeUInt(Self), 16);

  for var opponent := Low(TOpponent) to High(TOpponent) do
    AddRadioButton(opponent);

  fOpponent := CDefaultOpponent;

  fButtonMap[fOpponent].IsChecked := True;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.BeforeDestruction;
begin
  fButtonMap.Free;

  inherited;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.AddRadioButton(const aOpponent: TOpponent);
begin
  var button := TRadioButton.Create(Self);

  var w := CButtonWidths[aOpponent];

  button.Parent         := OptionLayout;
  button.Name           := 'rb' + COpponentNames[aOpponent].Replace(' ', '', [rfReplaceAll]);
  button.Text           := COpponentNames[aOpponent];
  button.GroupName      := fGroupName;
  button.Tag            := Ord(aOpponent);
  button.Width          := w;
  button.Margins.Right  := CButtonSpacing;
  button.Position.X     := Ord(aOpponent) * (w + CButtonSpacing);
  button.Align          := TAlignLayout.Left;
  button.OnChange       := RadioButtonChange;

  fButtonMap.Add(aOpponent, button);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.DoOpponentChange;
begin
  if Assigned(fOnOpponentChange) then
    fOnOpponentChange(Self);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.RadioButtonChange(Sender: TObject);
begin
  var button := Sender as TRadioButton;

  if not button.IsChecked then exit;

  var selected := TOpponent(button.Tag);

  if fOpponent = selected then exit;

  fOpponent := selected;

  DoOpponentChange;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TChallengers.SetOpponent(const aValue: TOpponent);
begin
  if fOpponent = aValue then exit;

  fOpponent := aValue;

  fButtonMap[fOpponent].IsChecked := True;

  DoOpponentChange;
end;

end.
