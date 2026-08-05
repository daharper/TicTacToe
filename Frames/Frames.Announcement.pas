unit Frames.Announcement;

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
  FMX.Controls.Presentation;

type
  TAnnouncement = class(TFrame)
    AnnouncementGroup: TRectangle;
    MessageLabel: TLabel;
    ButtonLayout: TLayout;
    btnOK: TButton;
    procedure btnOKClick(Sender: TObject);
  private
    { Private declarations }
    class var fInstance: TAnnouncement;
  public
    { Public declarations }
    procedure AfterConstruction; override;
    procedure BeforeDestruction; override;

    class procedure ShowMessage(const aParent: TControl; const aMessage: string);
  end;

implementation

{$R *.fmx}

{ TAnnouncement }

{----------------------------------------------------------------------------------------------------------------------}
procedure TAnnouncement.AfterConstruction;
begin
  inherited;

  // Wired here rather than in the .fmx, because the designer strips handlers it did not create itself.
  if Assigned(btnOK) then
    btnOK.OnClick := btnOKClick;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TAnnouncement.BeforeDestruction;
begin
  // The owner frees us, so let go of the class reference rather than leaving it dangling.
  if fInstance = Self then
    fInstance := nil;

  inherited;
end;

{----------------------------------------------------------------------------------------------------------------------}
class procedure TAnnouncement.ShowMessage(const aParent: TControl; const aMessage: string);
begin
  if not Assigned(fInstance) then
  begin
    fInstance        := TAnnouncement.Create(aParent);
    fInstance.Parent := aParent;
    fInstance.Align  := TAlignLayout.Center;
  end;

  fInstance.MessageLabel.Text := aMessage;

  fInstance.Visible := True;
  fInstance.BringToFront;
end;

{----------------------------------------------------------------------------------------------------------------------}
procedure TAnnouncement.btnOKClick(Sender: TObject);
begin
  Visible := False;
end;

end.
