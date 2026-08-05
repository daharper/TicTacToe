unit Core.Types;

interface

type
  TPieceKind = (pkNone = 0, pkX, pkO);

  TBoardPosition = (
    bpNone = -1,
    bpTopLeft = 0, bpTopCenter,    bpTopRight,
    bpMiddleLeft,  bpMiddleCenter, bpMiddleRight,
    bpBottomLeft,  bpBottomMiddle, bpBottomRight
  );

  TBoardPositions = TArray<TBoardPosition>;

  TBoardState = array [TBoardPosition] of TPieceKind;

  TOpponent = (opTipsy, opGenghis, opBoris, opSimaYi);

  TWinningLine = array[0..2] of TBoardPosition;

const
  CCornerPositions: array[0..3] of TBoardPosition = (
    bpTopLeft,
    bpTopRight,
    bpBottomLeft,
    bpBottomRight
  );

  CSidePositions: array[0..3] of TBoardPosition = (
    bpTopCenter,
    bpMiddleLeft,
    bpMiddleRight,
    bpBottomMiddle
  );

  COppositeCorners: array[bpTopLeft..bpBottomRight] of TBoardPosition = (
    bpBottomRight, // TopLeft
    bpNone,        // TopCenter
    bpBottomLeft,  // TopRight
    bpNone,        // MiddleLeft
    bpNone,        // MiddleCenter
    bpNone,        // MiddleRight
    bpTopRight,    // BottomLeft
    bpNone,        // BottomMiddle
    bpTopLeft      // BottomRight
  );

  CPieceNames: array [TBoardPosition] of string = (
    '',
    'TopLeft',    'TopCenter',    'TopRight',
    'MiddleLeft', 'MiddleCenter', 'MiddleRight',
    'BottomLeft', 'BottomMiddle', 'BottomRight'
  );

  CPieceLabels: array [TBoardPosition] of string = (
    '',
    'Top Left',    'Top Center',    'Top Right',
    'Middle Left', 'Middle Center', 'Middle Right',
    'Bottom Left', 'Bottom Middle', 'Bottom Right'
  );

  COpponentNames: array [TOpponent] of string = ('Tipsy', 'Genghis', 'Boris', 'Sima Yi');

  CPositionCount = 9;

implementation

end.
