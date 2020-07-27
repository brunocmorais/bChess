namespace bChess
{
    public static class Castling
    {
        public static bool CanCastleKingside(ChessBoard board, Color color)
        {
            if ((color == Color.White && !board.CastlingInfo.WhiteCanCastleKingside) || 
                (color == Color.Black && !board.CastlingInfo.BlackCanCastleKingside))
                return false;

            if (color == Color.White && (board.AllPieces & Constants.WhiteKingsideCastleMask) != 0)
                return false;

            if (color == Color.Black && (board.AllPieces & Constants.BlackKingsideCastleMask) != 0)
                return false;

            return true;
        }

        public static bool CanCastleQueenside(ChessBoard board, Color color)
        {
            if ((color == Color.White && !board.CastlingInfo.WhiteCanCastleQueenside) || 
                (color == Color.Black && !board.CastlingInfo.BlackCanCastleQueenside))
                return false;

            if (color == Color.White && (board.AllPieces & Constants.WhiteQueensideCastleMask) != 0)
                return false;

            if (color == Color.Black && (board.AllPieces & Constants.BlackQueensideCastleMask) != 0)
                return false;

            return true;
        }

        public static void HandleCastling(ChessBoard board, Piece piece, byte from, byte to)
        {
            Color color = board.NextTurn == Color.Black ? Color.White : Color.Black;

            if (piece == Piece.King)
            {
                byte rookFrom = 0xFF, rookTo = 0xFF;

                if (color == Color.White)
                {
                    if (from == 0x04 && to == 0x06) // roque curto das brancas
                    {
                        rookFrom = 0x07; rookTo = 0x05;
                    }
                    if (from == 0x04 && to == 0x02) // roque longo das brancas
                    {
                        rookFrom = 0x00; rookTo = 0x03;
                    }
                }
                else
                {
                    if (from == 0x74 && to == 0x76) // roque curto das pretas
                    {
                        rookFrom = 0x77; rookTo = 0x75;
                    }
                    if (from == 0x74 && to == 0x72) // roque longo das pretas
                    {
                        rookFrom = 0x70; rookTo = 0x73;
                    }
                }

                if (rookFrom != 0xFF && rookTo != 0xFF)
                {
                    ZobristHash.UpdateHash(board, rookFrom, rookTo);
                    Bitboard bitboard;

                    if (color == Color.White)
                        bitboard = board.WhiteRooks;
                    else
                        bitboard = board.BlackRooks;
                        
                    board.SetBitboard(new Bitboard(bitboard.Reset(rookFrom).Set(rookTo), Piece.Rook, color));
                }

                // desabilitando roques ao movimentar rei
                if (color == Color.White)
                {
                    board.CastlingInfo.WhiteCanCastleKingside = false;
                    board.CastlingInfo.WhiteCanCastleQueenside = false;
                }
                else
                {
                    board.CastlingInfo.BlackCanCastleKingside = false;
                    board.CastlingInfo.BlackCanCastleQueenside = false;
                }
            }

            if (board.CastlingInfo.WhiteCanCastleQueenside && (from == 0x00 || to == 0x00))
                board.CastlingInfo.WhiteCanCastleQueenside = false;

            if (board.CastlingInfo.WhiteCanCastleKingside && (from == 0x07 || to == 0x07))
                board.CastlingInfo.WhiteCanCastleKingside = false;

            if (board.CastlingInfo.BlackCanCastleQueenside && (from == 0x70 || to == 0x70))
                board.CastlingInfo.BlackCanCastleQueenside = false;

            if (board.CastlingInfo.BlackCanCastleKingside && (from == 0x77 || to == 0x77))
                board.CastlingInfo.BlackCanCastleKingside = false;
        }
    }
}