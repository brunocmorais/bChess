using System;
using System.Diagnostics;
using System.Text;

namespace bChess
{
    public class ChessBoard
    {
        /*
        Tabuleiro 0x88

        70 	71 	72 	73 	74 	75 	76 	77  ||	78 	79 	7A 	7B 	7C 	7D 	7E 	7F
        60 	61 	62 	63 	64 	65 	66 	67  ||	68 	69 	6A 	6B 	6C 	6D 	6E 	6F
        50 	51 	52 	53 	54 	55 	56 	57  ||	58 	59 	5A 	5B 	5C 	5D 	5E 	5F
        40 	41 	42 	43 	44 	45 	46 	47  ||	48 	49 	4A 	4B 	4C 	4D 	4E 	4F
        30 	31 	32 	33 	34 	35 	36 	37  ||	38 	39 	3A 	3B 	3C 	3D 	3E 	3F
        20 	21 	22 	23 	24 	25 	26 	27  ||	28 	29 	2A 	2B 	2C 	2D 	2E 	2F
        10 	11 	12 	13 	14 	15 	16 	17  ||	18 	19 	1A 	1B 	1C 	1D 	1E 	1F
        00 	01 	02 	03 	04 	05 	06 	07  ||	08 	09 	0A 	0B 	0C 	0D 	0E 	0F 
        */

        public Bitboard WhitePawns { get; private set; }
        public Bitboard WhiteKnights { get; private set; }
        public Bitboard WhiteBishops { get; private set; }
        public Bitboard WhiteRooks { get; private set; }
        public Bitboard WhiteQueens { get; private set; }
        public Bitboard WhiteKing { get; private set; }
        public Bitboard BlackPawns { get; private set; }
        public Bitboard BlackKnights { get; private set; }
        public Bitboard BlackBishops { get; private set; }
        public Bitboard BlackRooks { get; private set; }
        public Bitboard BlackQueens { get; private set; }
        public Bitboard BlackKing { get; private set; }
        public CastlingInfo CastlingInfo { get; private set; }
        public Color NextTurn { get; private set; }
        public byte From { get; private set; }
        public byte To { get; private set; }
        public CaptureInfo CaptureInfo { get; private set; }
        public ulong Hash { get; set; }
        public string Move => $"{GetMove(From)}{GetMove(To)}";

        public ChessBoard()
        {
            WhitePawns   = new Bitboard(Constants.WhitePawnStartPosition  , Piece.Pawn  , Color.White);
            WhiteKnights = new Bitboard(Constants.WhiteKnightStartPosition, Piece.Knight, Color.White);
            WhiteBishops = new Bitboard(Constants.WhiteBishopStartPosition, Piece.Bishop, Color.White);
            WhiteRooks   = new Bitboard(Constants.WhiteRookStartPosition  , Piece.Rook  , Color.White);
            WhiteQueens  = new Bitboard(Constants.WhiteQueenStartPosition , Piece.Queen , Color.White);
            WhiteKing    = new Bitboard(Constants.WhiteKingStartPosition  , Piece.King  , Color.White);
            BlackPawns   = new Bitboard(Constants.BlackPawnStartPosition  , Piece.Pawn  , Color.Black);
            BlackKnights = new Bitboard(Constants.BlackKnightStartPosition, Piece.Knight, Color.Black);
            BlackBishops = new Bitboard(Constants.BlackBishopStartPosition, Piece.Bishop, Color.Black);
            BlackRooks   = new Bitboard(Constants.BlackRookStartPosition  , Piece.Rook  , Color.Black);
            BlackQueens  = new Bitboard(Constants.BlackQueenStartPosition , Piece.Queen , Color.Black);
            BlackKing    = new Bitboard(Constants.BlackKingStartPosition  , Piece.King  , Color.Black);
            CastlingInfo = new CastlingInfo();
            CaptureInfo  = new CaptureInfo();
            From         = 0xFF;
            To           = 0xFF;
            NextTurn     = Color.White;
            Hash         = ZobristHash.ComputeHash(this);
        }

        public ChessBoard(ChessBoard chessBoard)
        {
            WhitePawns   = new Bitboard(chessBoard.WhitePawns  , Piece.Pawn  , Color.White);
            WhiteKnights = new Bitboard(chessBoard.WhiteKnights, Piece.Knight, Color.White);  
            WhiteBishops = new Bitboard(chessBoard.WhiteBishops, Piece.Bishop, Color.White);  
            WhiteRooks   = new Bitboard(chessBoard.WhiteRooks  , Piece.Rook  , Color.White);    
            WhiteQueens  = new Bitboard(chessBoard.WhiteQueens , Piece.Queen , Color.White);   
            WhiteKing    = new Bitboard(chessBoard.WhiteKing   , Piece.King  , Color.White);     
            BlackPawns   = new Bitboard(chessBoard.BlackPawns  , Piece.Pawn  , Color.Black);    
            BlackKnights = new Bitboard(chessBoard.BlackKnights, Piece.Knight, Color.Black);  
            BlackBishops = new Bitboard(chessBoard.BlackBishops, Piece.Bishop, Color.Black);  
            BlackRooks   = new Bitboard(chessBoard.BlackRooks  , Piece.Rook  , Color.Black);    
            BlackQueens  = new Bitboard(chessBoard.BlackQueens , Piece.Queen , Color.Black);   
            BlackKing    = new Bitboard(chessBoard.BlackKing   , Piece.King  , Color.Black);
            CastlingInfo = new CastlingInfo(chessBoard.CastlingInfo);
            CaptureInfo  = new CaptureInfo(chessBoard.CaptureInfo);
            NextTurn     = chessBoard.NextTurn == Color.White ? Color.Black : Color.White;
            From         = chessBoard.From;
            To           = chessBoard.To;
            Hash         = chessBoard.Hash;
        }

        public BaseBitboard WhitePieces => WhitePawns | WhiteKnights | WhiteBishops | WhiteRooks | WhiteQueens | WhiteKing;
        public BaseBitboard BlackPieces => BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKing;
        public BaseBitboard AllPieces => WhitePieces | BlackPieces;

        public BaseBitboard GetPlayerPieces(Color color)
        {
            return color == Color.White ? WhitePieces : BlackPieces;
        }

        public BaseBitboard GetOpponentPieces(Color color)
        {
            return color == Color.White ? BlackPieces : WhitePieces;
        }

        public override string ToString()
        {
            return $"{Move} - {Hash} - {FEN.Write(this)}";
        }

        public override bool Equals(object obj)
        {
            return Hash == (obj as ChessBoard)?.Hash;
        }

        public override int GetHashCode()
        {
            return (int) Hash;
        }

        public Piece GetPiece(byte position)
        {
            if (WhitePieces.IsSet(position))
            {
                if (WhitePawns.IsSet(position))
                    return Piece.Pawn;
                else if (WhiteKnights.IsSet(position))
                    return Piece.Knight;
                else if (WhiteBishops.IsSet(position))
                    return Piece.Bishop;
                else if (WhiteRooks.IsSet(position))
                    return Piece.Rook;
                else if (WhiteQueens.IsSet(position))
                    return Piece.Queen;
                else
                    return Piece.King;
            }
            else if (BlackPieces.IsSet(position))
            {
                if (BlackPawns.IsSet(position))
                    return Piece.Pawn;
                else if (BlackKnights.IsSet(position))
                    return Piece.Knight;
                else if (BlackBishops.IsSet(position))
                    return Piece.Bishop;
                else if (BlackRooks.IsSet(position))
                    return Piece.Rook;
                else if (BlackQueens.IsSet(position))
                    return Piece.Queen;
                else
                    return Piece.King;
            }
            else
                return Piece.None;
        }

        public Bitboard GetBitboard(Piece piece, Color color)
        {
            if (color == Color.White)
            {
                switch (piece)
                {
                    case Piece.Pawn:   return WhitePawns;
                    case Piece.Knight: return WhiteKnights;
                    case Piece.Bishop: return WhiteBishops;
                    case Piece.Rook:   return WhiteRooks;
                    case Piece.Queen:  return WhiteQueens;
                    case Piece.King:   return WhiteKing;
                    default: return null;
                }
            }
            else
            {
                switch (piece)
                {
                    case Piece.Pawn:   return BlackPawns;
                    case Piece.Knight: return BlackKnights;
                    case Piece.Bishop: return BlackBishops;
                    case Piece.Rook:   return BlackRooks;
                    case Piece.Queen:  return BlackQueens;
                    case Piece.King:   return BlackKing;
                    default: return null;
                }
            }
        }

        public void SetBitboard(Bitboard bitboard)
        {
            if (bitboard.Color == Color.White)
            {
                switch (bitboard.Piece)
                {
                    case Piece.Pawn:   WhitePawns = bitboard; break;
                    case Piece.Knight: WhiteKnights = bitboard; break;
                    case Piece.Bishop: WhiteBishops = bitboard; break;
                    case Piece.Rook:   WhiteRooks = bitboard; break;
                    case Piece.Queen:  WhiteQueens = bitboard; break;
                    case Piece.King:   WhiteKing = bitboard; break;
                }
            }
            else
            {
                switch (bitboard.Piece)
                {
                    case Piece.Pawn:   BlackPawns = bitboard; break;
                    case Piece.Knight: BlackKnights = bitboard; break;
                    case Piece.Bishop: BlackBishops = bitboard; break;
                    case Piece.Rook:   BlackRooks = bitboard; break;
                    case Piece.Queen:  BlackQueens = bitboard; break;
                    case Piece.King:   BlackKing = bitboard; break;
                }
            }
        }

        public void ResetPositionForColor(byte position, Piece pieceType, Color color)
        {
            if (color == Color.White)
            {
                switch (pieceType)
                {
                    case Piece.Pawn:   WhitePawns   = new Bitboard(WhitePawns.Reset(position),   Piece.Pawn,   Color.White); break;
                    case Piece.Knight: WhiteKnights = new Bitboard(WhiteKnights.Reset(position), Piece.Knight, Color.White); break;
                    case Piece.Bishop: WhiteBishops = new Bitboard(WhiteBishops.Reset(position), Piece.Bishop, Color.White); break;
                    case Piece.Rook:   WhiteRooks   = new Bitboard(WhiteRooks.Reset(position),   Piece.Rook,   Color.White); break;
                    case Piece.Queen:  WhiteQueens  = new Bitboard(WhiteQueens.Reset(position),  Piece.Queen,  Color.White); break;
                    case Piece.King:   WhiteKing    = new Bitboard(WhiteKing.Reset(position),    Piece.King,   Color.White); break;
                }
            }
            else
            {
                switch (pieceType)
                {
                    case Piece.Pawn:   BlackPawns   = new Bitboard(BlackPawns.Reset(position),   Piece.Pawn,   Color.Black); break;
                    case Piece.Knight: BlackKnights = new Bitboard(BlackKnights.Reset(position), Piece.Knight, Color.Black); break;
                    case Piece.Bishop: BlackBishops = new Bitboard(BlackBishops.Reset(position), Piece.Bishop, Color.Black); break;
                    case Piece.Rook:   BlackRooks   = new Bitboard(BlackRooks.Reset(position),   Piece.Rook,   Color.Black); break;
                    case Piece.Queen:  BlackQueens  = new Bitboard(BlackQueens.Reset(position),  Piece.Queen,  Color.Black); break;
                    case Piece.King:   BlackKing    = new Bitboard(BlackKing.Reset(position),    Piece.King,   Color.Black); break;
                }
            }
        }

        public ChessBoard MakeMove(Piece piece, byte from, byte to, Piece promotionPiece = Piece.Queen)
        {
            var board = new ChessBoard(this);
            ZobristHash.UpdateHash(board, from, to);

            if (board.GetOpponentPieces(NextTurn).IsSet(to)) // houve captura
            {
                Piece capturedPiece = board.GetPiece(to);
                board.ResetPositionForColor(to, capturedPiece, board.NextTurn);
                CaptureInfo = new CaptureInfo(true, piece, capturedPiece);
            }
            else
                CaptureInfo = new CaptureInfo(false, Piece.None, Piece.None);

            board.SetBitboard(new Bitboard(GetBitboard(piece, NextTurn).Reset(from).Set(to), piece, NextTurn));
            
            Promotion.HandlePromotion(board, piece, to, promotionPiece);
            Castling.HandleCastling(board, piece, from, to);

            board.From = from;
            board.To = to;

            return board;
        }

        public void Clear()
        {
            WhitePawns   = new Bitboard(0, Piece.Pawn  , Color.White);
            WhiteKnights = new Bitboard(0, Piece.Knight, Color.White);
            WhiteBishops = new Bitboard(0, Piece.Bishop, Color.White);
            WhiteRooks   = new Bitboard(0, Piece.Rook  , Color.White);
            WhiteQueens  = new Bitboard(0, Piece.Queen , Color.White);
            WhiteKing    = new Bitboard(0, Piece.King  , Color.White);

            BlackPawns   = new Bitboard(0, Piece.Pawn  , Color.Black);
            BlackKnights = new Bitboard(0, Piece.Knight, Color.Black);
            BlackBishops = new Bitboard(0, Piece.Bishop, Color.Black);
            BlackRooks   = new Bitboard(0, Piece.Rook  , Color.Black);
            BlackQueens  = new Bitboard(0, Piece.Queen , Color.Black);
            BlackKing    = new Bitboard(0, Piece.King  , Color.Black);
        }

        public void DefineNextTurn(Color color)
        {
            NextTurn = color;
        }

        private string GetMove(byte position)
        {
            if (position == 0xFF)
                return "00";

            char first = (char)(97 + ((position & 0xF)));
		    char second = (char)(49 + ((position & 0xF0) >> 4));
            return $"{first}{second}";
        }

        public ChessBoard InsertNullMove()
        {
            var board = new ChessBoard(this);
            board.CaptureInfo = new CaptureInfo(false, Piece.None, Piece.None);
            board.From = 0xFF;
            board.To = 0xFF;
            return board;
        }
    }
}