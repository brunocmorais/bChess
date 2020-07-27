namespace bChess
{
    public class CaptureInfo
    {
        public bool IsCapture { get; set; }
        public Piece Capturer { get; set; }
        public Piece Captured { get; set; }

        public CaptureInfo()
        {
            IsCapture = false;
            Capturer = Piece.None;
            Captured = Piece.None;
        }

        public CaptureInfo(CaptureInfo captureInfo)
        {
            IsCapture = captureInfo.IsCapture;
            Captured = captureInfo.Captured;
            Capturer = captureInfo.Capturer;
        }

        public CaptureInfo(bool isCapture, Piece capturer, Piece captured)
        {
            IsCapture = isCapture;
            Capturer = capturer;
            Captured = captured;
        }

        public int GetCaptureDelta()
        {
            if (!IsCapture)
                return 0;

            return GetValueByPiece(Captured) - GetValueByPiece(Capturer);
        }

        private int GetValueByPiece(Piece piece)
        {
            switch (piece)
            {
                case Piece.Pawn:   return Constants.Pawn;
                case Piece.Knight: return Constants.Knight;
                case Piece.Bishop: return Constants.Bishop;
                case Piece.Rook:   return Constants.Rook;
                case Piece.Queen:  return Constants.Queen;
                case Piece.King:   return Constants.King;
                default: return 0; 
            }
        }
    }
}