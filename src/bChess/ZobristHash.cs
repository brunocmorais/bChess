using System;

namespace bChess
{
    public static class ZobristHash
    {
        private static ulong[,] ZobristTable = new ulong[64, 12];
        private static Random NumberGenerator = new Random();

        static ZobristHash()
        {
            for (int i = 0; i < ZobristTable.GetLength(0); i++)
            {
                for (int j = 0; j < ZobristTable.GetLength(1); j++)
                {
                    ulong value;

                    do
                        value = GetRandomNumber();
                    while (CheckIfRandomValueAlreadyExists(value));
                    
                    ZobristTable[i, j] = value;
                }
            }
        }

        private static bool CheckIfRandomValueAlreadyExists(ulong value)
        {
            for (int i = 0; i < ZobristTable.GetLength(0); i++)
                for (int j = 0; j < ZobristTable.GetLength(1); j++)
                    if (ZobristTable[i, j] == value)
                        return true;

            return false;
        }

        private static int IndexOf(Piece piece, Color color)
        {
            int number = 0;

            switch (piece)
            {
                case Piece.Pawn:   number = 0; break;
                case Piece.Knight: number = 1; break;
                case Piece.Bishop: number = 2; break;
                case Piece.Rook:   number = 3; break;
                case Piece.Queen:  number = 4; break;
                case Piece.King:   number = 5; break;
                default: return -1;
            }

            if (color == Color.Black)
                number += 6;

            return number;
        }

        private static ulong GetRandomNumber()
        {
            var num = NumberGenerator.NextDouble();
            return (ulong)(num * Int64.MaxValue);
        }

        public static ulong ComputeHash(ChessBoard board)
        {
            ulong hash = 0;
            
            for (int i = 0; i < Constants.BoardSize; i++)
            {
                byte position = Constants.Positions[i];
                Piece piece = board.GetPiece(position);

                if (piece != Piece.None)
                {
                    Color color = board.WhitePieces.IsSet(position) ? Color.White : Color.Black;
                    int index = IndexOf(piece, color);
                    hash ^= ZobristTable[i, index];
                }
            }

            return hash;
        }

        public static void UpdateHash(ChessBoard board, byte from, byte to)
        {
            Piece fromPiece = board.GetPiece(from);
            Piece toPiece = board.GetPiece(to);

            Color colorFrom = board.WhitePieces.IsSet(from) ? Color.White : Color.Black;
            Color colorTo = colorFrom == Color.White ? Color.Black : Color.White;
            // tirando a peça do lugar original
            board.Hash ^= ZobristTable[Bitboard.GetIndex(from), IndexOf(fromPiece, colorFrom)]; 

            // tirando a peça capturada, se houver
            if (toPiece != Piece.None)
                board.Hash ^= ZobristTable[Bitboard.GetIndex(to), IndexOf(toPiece, colorTo)]; 

            // colocando a peça em seu novo lugar
            board.Hash ^= ZobristTable[Bitboard.GetIndex(to), IndexOf(fromPiece, colorFrom)]; 
        }

        public static void UpdateHashForPromotion(ChessBoard board, byte position, Piece promotionPiece)
        {
            Color colorFrom = board.WhitePieces.IsSet(position) ? Color.White : Color.Black;
            // tirando o peão do lugar
            board.Hash ^= ZobristTable[Bitboard.GetIndex(position), IndexOf(Piece.Pawn, colorFrom)]; 
            // colocando a nova peça no lugar
            board.Hash ^= ZobristTable[Bitboard.GetIndex(position), IndexOf(promotionPiece, colorFrom)]; 
        }
    }
}