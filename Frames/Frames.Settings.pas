unit Frames.Settings;

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
  FMX.Layouts,
  FMX.StdCtrls,
  FMX.Objects,
  FMX.Controls.Presentation, System.Math.Vectors, FMX.Controls3D, FMX.Layers3D;

type
  TSettings = class(TFrame)
    Background: TRectangle;
    DurationLayout: TLayout;
    lblDuration: TLabel;
    tbDuration: TTrackBar;
    lblSeconds: TLabel;
    CentralLayout: TLayout;
    Layout1: TLayout;
    Layout2: TLayout;
    Label1: TLabel;
    lblScore: TLabel;
    procedure tbDurationChange(Sender: TObject);
  private
    fDuration: integer;
    fBestScore: integer;
    fPath: string;
  public
    property Duration: integer read fDuration;
    property BestScore: integer read fBestScore;

    procedure Load;
    procedure Save;
    procedure UpdateScore(const aScore: integer);

    procedure AfterConstruction; override;
  end;

implementation

{$R *.fmx}

uses
  System.IOUtils;

const
  CDefaultDuration = 20;

{ TSettings }

{----------------------------------------------------------------------------------------------------------------------}
procedure TSettings.tbDurationChange(Sender: TObject);
begin
  fDuration := Round(tbDuration.Value);
  tbDuration.Value := fDuration;
  lblSeconds.Text := IntToStr(Round(tbDuration.Value));
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TSettings.UpdateScore(const aScore: integer);
begin
  if aScore <= fBestScore then exit;

  fBestScore := aScore;
  lblScore.Text := IntToStr(fBestScore);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TSettings.AfterConstruction;
const
  CName = 'Settings.txt';
begin
  inherited;

  fPath := TPath.Combine(TPath.GetDocumentsPath, CName);
  fDuration := CDefaultDuration;

  lblSeconds.Text := IntToStr(CDefaultDuration);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TSettings.Load;

begin
  if not TFile.Exists(fPath) then exit;

  var text  := TFile.ReadAllText(fPath);
  var parts := text.Split(['|'], TStringSplitOptions.ExcludeEmpty);

  if Length(parts) = 0 then exit;

  if not TryStrToInt(parts[0], fDuration) then
    fDuration := CDefaultDuration;

  tbDuration.Value := fDuration;
  lblSeconds.Text := IntToStr(fDuration);

  if Length(parts) = 2 then
    if TryStrToInt(parts[1], fBestScore) then
      lblScore.Text := IntToStr(fBestScore);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TSettings.Save;
begin
  var text := Format('%d|%d', [fDuration, fBestScore]);

  TFile.WriteAllText(fPath, text);
end;

end.
