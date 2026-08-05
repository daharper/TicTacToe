unit Frames.Timer;

interface

uses
  System.SysUtils,
  System.Types,
  System.UITypes,
  System.Classes,
  System.Variants,
  FMX.Types,
  FMX.Controls,
  FMX.Forms,
  FMX.Graphics,
  FMX.Dialogs,
  FMX.StdCtrls,
  FMX.Controls.Presentation;

type
  TGameTimer = class(TFrame)
    CountdownLabel: TLabel;
  private
    { Private declarations }
    fTicker: TTimer;
    fDuration: Integer;
    fSecondsRemaining: Integer;
    fOnTimeUp: TNotifyEvent;

    procedure TickerTimer(Sender: TObject);
    procedure UpdateDisplay;
    procedure SetDuration(const aValue: integer);
  public
    { Public declarations }
    constructor Create(AOwner: TComponent); override;

    procedure Start;
    procedure Stop;

    property Duration: Integer read fDuration write SetDuration;
    property SecondsRemaining: Integer read fSecondsRemaining;
    property OnTimeUp: TNotifyEvent read fOnTimeUp write fOnTimeUp;
  end;

implementation

{$R *.fmx}

const
  CDuration     = 20;
  CTickInterval = 1000;

{ TGameTimer }

{----------------------------------------------------------------------------------------------------------------------}
constructor TGameTimer.Create(AOwner: TComponent);
begin
  inherited;

  fSecondsRemaining := CDuration;

  fTicker          := TTimer.Create(Self);
  fTicker.Enabled  := False;
  fTicker.Interval := CTickInterval;
  fTicker.OnTimer  := TickerTimer;

  UpdateDisplay;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGameTimer.Start;
begin
  fSecondsRemaining := fDuration;

  UpdateDisplay;

  fTicker.Enabled := True;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGameTimer.Stop;
begin
  fTicker.Enabled := False;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGameTimer.TickerTimer(Sender: TObject);
begin
  if fSecondsRemaining = 0 then exit;

  Dec(fSecondsRemaining);

  UpdateDisplay;

  if fSecondsRemaining > 0 then exit;

  Stop;

  if Assigned(fOnTimeUp) then
    fOnTimeUp(Self);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGameTimer.UpdateDisplay;
begin
  if not Assigned(CountdownLabel) then exit;

  CountdownLabel.Text := fSecondsRemaining.ToString;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TGameTimer.SetDuration(const aValue: Integer);
begin
  fDuration := aValue;
  CountdownLabel.Text := aValue.ToString;
end;

end.
