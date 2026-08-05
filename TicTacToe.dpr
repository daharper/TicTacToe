program TicTacToe;

uses
  System.StartUpCopy,
  FMX.Forms,
  Forms.Main in 'Forms\Forms.Main.pas' {MainForm},
  Frames.BoardBackground in 'Frames\Frames.BoardBackground.pas' {BoardBackground: TFrame},
  Modules.Main in 'Modules\Modules.Main.pas' {MainDataModule: TDataModule},
  Frames.Board in 'Frames\Frames.Board.pas' {Board: TFrame},
  Core.Types in 'Core\Core.Types.pas',
  Frames.Piece in 'Frames\Frames.Piece.pas' {Piece: TFrame},
  Core.Minimax in 'Core\Core.Minimax.pas',
  Core.Rules in 'Core\Core.Rules.pas',
  Frames.Challengers in 'Frames\Frames.Challengers.pas' {Challengers: TFrame},
  Frames.Timer in 'Frames\Frames.Timer.pas' {GameTimer: TFrame},
  Core.Players in 'Core\Core.Players.pas',
  Core.Tactics in 'Core\Core.Tactics.pas',
  Frames.Settings in 'Frames\Frames.Settings.pas' {Settings: TFrame},
  Frames.Announcement in 'Frames\Frames.Announcement.pas' {Announcement: TFrame},
  Core.Game in 'Core\Core.Game.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TMainDataModule, MainDataModule);
  Application.CreateForm(TMainForm, MainForm);
  Application.Run;
end.
