namespace bChess
{
    public class Bitboard : BaseBitboard
    {
        public Bitboard(ulong bitboard, Piece piece, Color color) : base(bitboard) 
        {
            Piece = piece;
            Color = color;
        }

        public Bitboard(BaseBitboard bitboard, Piece piece, Color color) : base(bitboard) 
        {
            Piece = piece;
            Color = color;
        }

        public Piece Piece { get; }
        public Color Color { get; }
    }
}