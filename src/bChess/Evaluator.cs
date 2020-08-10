using System.Diagnostics;
using System.Linq;

namespace bChess
{
    public static class Evaluator
    {
        public static int Evaluate(ChessBoard board)
        {
            int whites = (board.WhitePawns.Count * Constants.Pawn)     + (board.WhiteKnights.Count * Constants.Knight) +
                         (board.WhiteBishops.Count * Constants.Bishop) + (board.WhiteRooks.Count * Constants.Rook)     +
                         (board.WhiteQueens.Count * Constants.Queen)   + (board.WhiteKing.Count * Constants.King);

            int blacks = (board.BlackPawns.Count * Constants.Pawn)     + (board.BlackKnights.Count * Constants.Knight) +
                         (board.BlackBishops.Count * Constants.Bishop) + (board.BlackRooks.Count * Constants.Rook)     +
                         (board.BlackQueens.Count * Constants.Queen)   + (board.BlackKing.Count * Constants.King);

            int whitePositions = 0, blackPositions = 0;

            for (int i = 0; i < Constants.BoardSize; i++)
            {
                ulong mask = 0x1ul << i;

                if ((board.WhitePawns & mask) == mask)
                    whitePositions += Constants.WhitePawnPositions[i];

                if ((board.BlackPawns & mask) == mask)
                    blackPositions += Constants.BlackPawnPositions[i];

                if ((board.WhiteKnights & mask) == mask)
                    whitePositions += Constants.WhiteKnightPositions[i];

                if ((board.BlackKnights & mask) == mask)
                    blackPositions += Constants.BlackKnightPositions[i];

                if ((board.WhiteBishops & mask) == mask)
                    whitePositions += Constants.WhiteBishopPositions[i];

                if ((board.BlackBishops & mask) == mask)
                    blackPositions += Constants.BlackBishopPositions[i];

                if ((board.WhiteRooks & mask) == mask)
                    whitePositions += Constants.WhiteRookPositions[i];

                if ((board.BlackRooks & mask) == mask)
                    blackPositions += Constants.BlackRookPositions[i];

                if ((board.WhiteQueens & mask) == mask)
                    whitePositions += Constants.WhiteQueenPositions[i];

                if ((board.BlackQueens & mask) == mask)
                    blackPositions += Constants.BlackQueenPositions[i];

                if ((board.WhiteKing & mask) == mask)
                    whitePositions += Constants.WhiteKingPositions[i];

                if ((board.BlackKing & mask) == mask)
                    blackPositions += Constants.BlackKingPositions[i];
            }

            return (whites - blacks) + (whitePositions - blackPositions);
        }

        public static bool IsInCheck(ChessBoard board)
        {
            var board1 = board.InsertNullMove();
            var moves = MoveGenerator.GetPseudoMovesForPosition(board1);
            var kingBitboard = board.GetBitboard(Piece.King, board.NextTurn);

            ulong mask = 0x1ul;
            byte kingPosition = 0;

            for (byte i = 0; i < Constants.BoardSize; i++)
            {
                if (i != 0)
                    mask <<= 1;

                if ((kingBitboard & mask) == mask)
                {
                    kingPosition = Constants.Positions[i];
                    break;
                }
            }

            return moves.Any(x => x.To == kingPosition);
        }
    }
}