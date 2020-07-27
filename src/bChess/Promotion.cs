namespace bChess
{
    public static class Promotion
    {
        public static void HandlePromotion(ChessBoard board, Piece piece, byte to, Piece promotionPiece)
        {
            int rank = (to >> 4);
            Color color = board.NextTurn == Color.Black ? Color.White : Color.Black;

            if (piece == Piece.Pawn && (rank == 0 || rank == 7)) // promoção
            {
                ZobristHash.UpdateHashForPromotion(board, to, promotionPiece);
                board.ResetPositionForColor(to, Piece.Pawn, color);
                board.SetBitboard(new Bitboard(board.GetBitboard(promotionPiece, color).Set(to), promotionPiece, color));
            }
        }
    }
}