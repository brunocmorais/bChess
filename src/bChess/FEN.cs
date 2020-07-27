using System;
using System.Text;

namespace bChess
{
    public static class FEN
    {
        public static ChessBoard Read(string fen)
        {
            string[] fenParts = fen.Trim().Split(" ");
            string board = fenParts[0];
            string nextTurn = fenParts[1];
            string castling = fenParts[2];
            int pointer = Constants.BoardSize - 1;
            var chessBoard = new ChessBoard();
            chessBoard.Clear();

            foreach (char c in board)
            {
                if (c == '/')
                    continue;

                if (int.TryParse(c.ToString(), out int result))
                    pointer -= result;
                else
                {
                    byte position = Constants.Positions[pointer];

                    Bitboard pieces;

                    switch (c)
                    {
                        case 'p': pieces = chessBoard.BlackPawns;   break; 
                        case 'n': pieces = chessBoard.BlackKnights; break;
                        case 'b': pieces = chessBoard.BlackBishops; break;
                        case 'r': pieces = chessBoard.BlackRooks;   break; 
                        case 'q': pieces = chessBoard.BlackQueens;  break;
                        case 'k': pieces = chessBoard.BlackKing;    break; 
                        case 'P': pieces = chessBoard.WhitePawns;   break; 
                        case 'N': pieces = chessBoard.WhiteKnights; break;
                        case 'B': pieces = chessBoard.WhiteBishops; break;
                        case 'R': pieces = chessBoard.WhiteRooks;   break; 
                        case 'Q': pieces = chessBoard.WhiteQueens;  break;
                        case 'K': pieces = chessBoard.WhiteKing;    break; 
                        default: throw new ArgumentException();
                    }

                    chessBoard.SetBitboard(new Bitboard(pieces.Set(position), pieces.Piece, pieces.Color));
                    pointer--;
                }
            }

            chessBoard.DefineNextTurn(nextTurn == "w" ? Color.White : Color.Black);
            
            chessBoard.CastlingInfo.WhiteCanCastleKingside  = castling.Contains("K");
            chessBoard.CastlingInfo.BlackCanCastleKingside  = castling.Contains("k");
            chessBoard.CastlingInfo.WhiteCanCastleQueenside = castling.Contains("Q");
            chessBoard.CastlingInfo.BlackCanCastleQueenside = castling.Contains("q");

            chessBoard.Hash = ZobristHash.ComputeHash(chessBoard);
            // TODO: en passant, controle de movimentos

            return chessBoard; 
        }

        public static string Write(ChessBoard board)
        {
            var sb = new StringBuilder();
            int emptyCounter = 0;

            for (int i = Constants.BoardSize - 1; i >= 0; i--)
            {
                if ((i + 1) % 8 == 0 && (i + 1) != Constants.BoardSize && i != 0)
                {
                    if (emptyCounter > 0)
                    {
                        sb.Append(emptyCounter);
                        emptyCounter = 0;
                    }
                    
                    sb.Append("/");
                }

                byte position = Constants.Positions[i];
                var piece = board.GetPiece(position);

                if (piece == Piece.None)
                    emptyCounter++;
                else
                {
                    if (emptyCounter > 0)
                    {
                        sb.Append(emptyCounter);
                        emptyCounter = 0;
                    }

                    char c;

                    switch (piece)
                    {
                        case Piece.Pawn:   c = 'p'; break;
                        case Piece.Knight: c = 'n'; break;
                        case Piece.Bishop: c = 'b'; break;
                        case Piece.Rook:   c = 'r'; break;
                        case Piece.Queen:  c = 'q'; break;
                        case Piece.King:   c = 'k'; break;
                        default: throw new ArgumentException();
                    }

                    if (board.WhitePieces.IsSet(position))
                        sb.Append(char.ToUpper(c));
                    else
                        sb.Append(c);
                }
            }

            if (emptyCounter > 0)
                sb.Append(emptyCounter);

            if (board.NextTurn == Color.Black)
                sb.Append(" b");
            else
                sb.Append(" w");

            sb.Append(" KQkq - 0 1"); // TODO: alterar ao implementar roque, en passant e contagem de moves

            return sb.ToString();
        }
    }
}