unit Forms.Main;

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
  FMX.Ani,
  FMX.Layouts,
  FMX.Controls.Presentation,
  FMX.StdCtrls,
  Core.Types,
  Core.Game,
  Frames.Board,
  Frames.Announcement,
  Frames.BoardBackground,
  Frames.Challengers,
  Frames.Settings,
  Frames.Timer,
  Frames.Piece,
  Modules.Main;

type
  TMainForm = class(TForm)
    ControlLayout: TLayout;
    OpponentLayout: TLayout;
    BoardLayout: TLayout;
    MainLayout: TLayout;
    NewGameLayout: TLayout;
    btnNew: TButton;
    SettingsLayout: TLayout;
    TimerLayout: TLayout;
    btnSettings: TButton;
    procedure btnSettingsClick(Sender: TObject);
    procedure btnStartClick(Sender: TObject);
  private
    fBoardBackground: TBoardBackground;
    fBoard: TBoard;
    fChallengers: TChallengers;
    fSettings: TSettings;
    fSettingsSlide: TFloatAnimation;
    fSettingsShown: Boolean;
    fTimer: TGameTimer;
    fWobble: TFloatAnimation;
    fWobblesLeft: Integer;
    fGame: IGame;

    function SettingsTop(const aShown: Boolean): Single;

    procedure OnPlayerVictory;
    procedure OnDraw;
    procedure OnAiVictory;

    procedure OnTimeUp(Sender: TObject);
    procedure OnOpponentChange(Sender: TObject);
    procedure SlideSettings(const aShow: Boolean);
    procedure WobbleBoard;
    procedure WobbleFinished(Sender: TObject);
    procedure InitializeComponents;
    procedure InitializeGame;
    procedure OnPieceSelected(const aPiece: TPiece);
    procedure OnAiTurn;
  public
    constructor Create(AOwner: TComponent); override;
  end;

var
  MainForm: TMainForm;

implementation

{$R *.fmx}

const
  CWobbleAngle    = 3.0;
  CWobbleDuration = 1.2;
  CWobbleRepeats  = 2;

  CSettingsGap      = 4.0;
  CSettingsDuration = 0.3;

{----------------------------------------------------------------------------------------------------------------------}
constructor TMainForm.Create(AOwner: TComponent);
begin
  inherited;

  InitializeComponents;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.btnStartClick(Sender: TObject);
begin
  fTimer.Stop;

  if fSettingsShown then
  begin
    fSettingsShown := false;
    SlideSettings(false);
  end;

  InitializeGame;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnOpponentChange(Sender: TObject);
begin
  fTimer.Stop;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnTimeUp(Sender: TObject);
begin
  if fGame.State <> gsActive then exit;

  fGame.Stop;

  WobbleBoard;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.btnSettingsClick(Sender: TObject);
begin
  fTimer.Stop;

  fSettingsShown := not fSettingsShown;

  SlideSettings(fSettingsShown);
end;

{ Settings Animation }

{----------------------------------------------------------------------------------------------------------------------}
function TMainForm.SettingsTop(const aShown: Boolean): Single;
begin
  if not aShown then
    exit(MainLayout.Height);

  var buttonBottom := btnSettings.LocalToAbsolute(TPointF.Create(0, btnSettings.Height));

  Result := MainLayout.AbsoluteToLocal(buttonBottom).Y + CSettingsGap;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.SlideSettings(const aShow: Boolean);
begin
  if not aShow then
    fSettings.Save;

  fSettingsSlide.StopAtCurrent;

  fSettings.Width      := MainLayout.Width;
  fSettings.Position.X := 0;

  fSettingsSlide.StopValue := SettingsTop(aShow);

  fSettings.BringToFront;
  fSettingsSlide.Start;
end;

{ Grid Wobble Effect }

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.WobbleBoard;
begin
  fWobblesLeft := CWobbleRepeats;
  fWobble.Start;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.WobbleFinished(Sender: TObject);
begin
  Dec(fWobblesLeft);

  if fWobblesLeft > 0 then
  begin
    TThread.ForceQueue(nil, procedure begin fWobble.Start end);
    Exit;
  end;

  BoardLayout.RotationAngle := 0;

  TAnnouncement.ShowMessage(MainLayout, 'Time Up!');
end;

{ Game Management }

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.InitializeGame;
begin
  fGame := TGame.Start(fSettings.Duration, fChallengers.Opponent);

  fBoard.Initialize(fGame.Board);

  fTimer.Duration := fSettings.Duration;
  fTimer.Start;

  if fGame.IsAiTurn then
    OnAiTurn;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnAiTurn;
var
  move: TBoardPosition;
begin
  var state := fGame.MoveAi(move);

  fBoard.UpdatePiece(move, fGame.Ai.PieceKind);

  case state of
    gsAiWon:
      OnAiVictory;
    gsDraw:
      OnDraw;
    else
      fGame.NextTurn;
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnPieceSelected(const aPiece: TPiece);
begin
  if not Assigned(fGame) then exit;
  if not fGame.IsPlayerTurn then exit;

  var state := fGame.MovePlayer(aPiece.BoardPosition);

  fBoard.UpdatePiece(aPiece.BoardPosition, fGame.User.PieceKind);

  case state of
    gsPlayerWon:
      OnPlayerVictory;
    gsDraw:
      OnDraw;
    else
    begin
      fGame.NextTurn;
      OnAiTurn;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnPlayerVictory;
begin
  fTimer.Stop;

  var msg   := 'You Won!';
  var score := fGame.CalculateScore;

  if score > fSettings.BestScore then
  begin
    fSettings.UpdateScore(score);
    msg := msg + ' New High Score: ' + IntToStr(score);
  end;

  TAnnouncement.ShowMessage(MainLayout, msg);
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnAiVictory;
begin
  fTimer.Stop;
  TAnnouncement.ShowMessage(MainLayout, 'You Lost...');
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.OnDraw;
begin
  fTimer.Stop;
  TAnnouncement.ShowMessage(MainLayout, 'A Draw.');
end;

{ Initialization }

{----------------------------------------------------------------------------------------------------------------------}
procedure TMainForm.InitializeComponents;
begin
  fBoardBackground := TBoardBackground.Create(Self);
  fBoardBackground.Parent := BoardLayout;
  fBoardBackground.Align := TAlignLayout.Client;

  fBoard := TBoard.Create(Self);
  fBoard.Parent := BoardLayout;
  fBoard.Align := TAlignLayout.Client;
  fBoard.BringToFront;
  fBoard.OnSelection := OnPieceSelected;

  fWobble               := TFloatAnimation.Create(Self);
  fWobble.Parent        := BoardLayout;
  fWobble.PropertyName  := 'RotationAngle';
  fWobble.AnimationType := TAnimationType.Out;
  fWobble.Interpolation := TInterpolationType.Elastic;
  fWobble.StartValue    := CWobbleAngle;
  fWobble.StopValue     := 0;
  fWobble.Duration      := CWobbleDuration;
  fWobble.OnFinish      := WobbleFinished;

  fTimer := TGameTimer.Create(Self);
  fTimer.Parent := TimerLayout;
  fTimer.Align := TAlignLayout.TopCenter;
  fTimer.BringToFront;
  fTimer.OnTimeUp := OnTimeUp;

  fChallengers := TChallengers.Create(Self);
  fChallengers.Parent := OpponentLayout;
  fChallengers.Align := TAlignLayout.Client;
  fChallengers.OnOpponentChange := OnOpponentChange;

  fSettingsShown := False;

  fSettings            := TSettings.Create(Self);
  fSettings.Parent     := MainLayout;
  fSettings.Align      := TAlignLayout.None;
  fSettings.Width      := MainLayout.Width;
  fSettings.Position.X := 0;
  fSettings.Position.Y := SettingsTop(fSettingsShown);
  fSettings.Load;

  fSettingsSlide                  := TFloatAnimation.Create(Self);
  fSettingsSlide.Parent           := fSettings;
  fSettingsSlide.PropertyName     := 'Position.Y';
  fSettingsSlide.AnimationType    := TAnimationType.Out;
  fSettingsSlide.Interpolation    := TInterpolationType.Quadratic;
  fSettingsSlide.StartFromCurrent := True;
  fSettingsSlide.Duration         := CSettingsDuration;

  fTimer.Duration := fSettings.Duration;
end;

end.
