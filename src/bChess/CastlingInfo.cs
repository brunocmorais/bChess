namespace bChess
{
    public class CastlingInfo
    {
        public bool WhiteCanCastleKingside { get; set; }
        public bool BlackCanCastleKingside { get; set; }
        public bool WhiteCanCastleQueenside { get; set; }
        public bool BlackCanCastleQueenside { get; set; }

        public CastlingInfo()
        {
            WhiteCanCastleKingside  = true;
            BlackCanCastleKingside  = true;
            WhiteCanCastleQueenside = true;
            BlackCanCastleQueenside = true;
        }

        public CastlingInfo(CastlingInfo castlingInfo)
        {
            WhiteCanCastleKingside  = castlingInfo.WhiteCanCastleKingside; 
            BlackCanCastleKingside  = castlingInfo.BlackCanCastleKingside; 
            WhiteCanCastleQueenside = castlingInfo.WhiteCanCastleQueenside; 
            BlackCanCastleQueenside = castlingInfo.BlackCanCastleQueenside; 
        }
    }
}