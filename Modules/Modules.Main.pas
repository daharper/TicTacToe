unit Modules.Main;

interface

uses
  System.SysUtils, System.Classes, FMX.Types, FMX.Controls;

type
  TMainDataModule = class(TDataModule)
    MBPStyleBook: TStyleBook;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  MainDataModule: TMainDataModule;

implementation

{%CLASSGROUP 'FMX.Controls.TControl'}

{$R *.dfm}

end.
