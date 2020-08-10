using System;
using System.Collections.Generic;
using System.Linq;

namespace bChess
{
    public static class MoveGenerator
    {
        public static IEnumerable<ChessBoard> GetPseudoMovesForPosition(ChessBoard board)
        {
            var pieces = board.GetPlayerPieces(board.NextTurn);

            for (int i = 0; i < Constants.BoardSize; i++)
            {
                var pos = Constants.Positions[i];

                if (pieces.IsSet(pos))
                {
                    var moves = MoveGenerator.GenerateMoves(board, pos);

                    foreach (var move in moves)
                    {
                        if (move.WhiteKing == 0 || move.BlackKing == 0) // movimento inválido
                        {
                            yield return null;
                            yield break;
                        }

                        yield return move;
                    }
                }
            }
        }

        public static IEnumerable<ChessBoard> GenerateMoves(ChessBoard board, byte position)
        {
            Piece piece = board.GetPiece(position);

            switch (piece)
            {
                case Piece.Pawn: return GeneratePawnMoves(board, position);
                case Piece.Knight: return GenerateKnightMoves(board, position);
                case Piece.Bishop: return GenerateBishopMoves(board, position);
                case Piece.Rook: return GenerateRookMoves(board, position);
                case Piece.Queen: return GenerateQueenMoves(board, position);
                case Piece.King: return GenerateKingMoves(board, position);
                default:
                    return Enumerable.Empty<ChessBoard>();
            }
        }

        private static IEnumerable<ChessBoard> GeneratePawnMoves(ChessBoard board, byte position)
        {
            Color color = board.WhitePawns.IsSet(position) ? Color.White : Color.Black;
            BaseBitboard playerPieces = board.GetPlayerPieces(color);
            BaseBitboard opponentPieces = board.GetOpponentPieces(color);
            BaseBitboard allPieces = board.AllPieces;
            byte rotatedPosition = position;
            bool rotated = false;

            if (color == Color.Black)
            {
                playerPieces = playerPieces.Rotate();
                opponentPieces = opponentPieces.Rotate();
                allPieces = playerPieces | opponentPieces;
                rotatedPosition = BaseBitboard.RotatePosition(position);
                rotated = true;
            }

            byte stepAhead = (byte)(rotatedPosition + 0x10);

            if (!allPieces.IsSet(stepAhead)) // tem uma casa livre a frente 
            {
                yield return board.MakeMove(Piece.Pawn, position, rotated ? BaseBitboard.RotatePosition(stepAhead) : stepAhead);

                if ((rotatedPosition >> 4) == 1) // peão ainda não movimentado
                {
                    byte twoStepsAhead = (byte)(rotatedPosition + 0x20);

                    if (!allPieces.IsSet(twoStepsAhead)) // tem espaço para andar duas casas a frente
                        yield return board.MakeMove(Piece.Pawn, position, rotated ? BaseBitboard.RotatePosition(twoStepsAhead) : twoStepsAhead);
                }
            }

            byte leftCapture = (byte)((rotatedPosition + 0xF));
            byte rightCapture = (byte)((rotatedPosition + 0x11));

            if ((leftCapture & 0x88) == 0 && opponentPieces.IsSet(leftCapture)) // captura à esquerda
                yield return board.MakeMove(Piece.Pawn, position, rotated ? BaseBitboard.RotatePosition(leftCapture) : leftCapture);

            if ((rightCapture & 0x88) == 0 && opponentPieces.IsSet(rightCapture)) // captura à direita
                yield return board.MakeMove(Piece.Pawn, position, rotated ? BaseBitboard.RotatePosition(rightCapture) : rightCapture);

            // TODO: en passant
        }

        private static IEnumerable<ChessBoard> GenerateKnightMoves(ChessBoard board, byte position)
        {
            Color color = board.WhiteKnights.IsSet(position) ? Color.White : Color.Black;
            BaseBitboard playerPieces = board.GetPlayerPieces(color);

            foreach (var possibility in Constants.KnightPossibilities)
            {
                int candidate = position + possibility;

                if (candidate >= 0 && (candidate & 0x88) == 0 && !playerPieces.IsSet((byte)candidate))
                    yield return board.MakeMove(Piece.Knight, position, (byte)candidate);
            }
        }

        private static IEnumerable<ChessBoard> GenerateBishopMoves(ChessBoard board, byte position)
        {
            return GenerateSlidingPieceMoves(board, position, Piece.Bishop);
        }

        private static IEnumerable<ChessBoard> GenerateRookMoves(ChessBoard board, byte position)
        {
            return GenerateSlidingPieceMoves(board, position, Piece.Rook);
        }

        private static IEnumerable<ChessBoard> GenerateQueenMoves(ChessBoard board, byte position)
        {
            return GenerateSlidingPieceMoves(board, position, Piece.Queen);
        }

        private static IEnumerable<ChessBoard> GenerateKingMoves(ChessBoard board, byte position)
        {
            Color color = board.WhiteKing.IsSet(position) ? Color.White : Color.Black;
            BaseBitboard playerPieces = board.GetPlayerPieces(color);

            foreach (var possibility in Constants.KingPossibilities)
            {
                int candidate = position + possibility;

                if (candidate >= 0 && (candidate & 0x88) == 0 && !playerPieces.IsSet((byte)candidate))
                    yield return board.MakeMove(Piece.King, position, (byte)candidate);
            }

            if (Castling.CanCastleKingside(board, color))
            {
                if (color == Color.White)
                    yield return board.MakeMove(Piece.King, 0x04, 0x06);
                else
                    yield return board.MakeMove(Piece.King, 0x74, 0x76);
            }

            if (Castling.CanCastleQueenside(board, color))
            {
                if (color == Color.White)
                    yield return board.MakeMove(Piece.King, 0x04, 0x02);
                else
                    yield return board.MakeMove(Piece.King, 0x74, 0x72);
            }
        }

        private static IEnumerable<ChessBoard> GenerateSlidingPieceMoves(ChessBoard board, byte position, Piece piece)
        {
            int[] possibilities;
            Color color;

            switch (piece)
            {
                case Piece.Bishop: 
                    possibilities = Constants.BishopPossibilities;
                    color = board.WhiteBishops.IsSet(position) ? Color.White : Color.Black;
                    break;
                case Piece.Rook: 
                    possibilities = Constants.RookPossibilities;
                    color = board.WhiteRooks.IsSet(position) ? Color.White : Color.Black;
                    break;
                case Piece.Queen: 
                    possibilities = Constants.QueenPossibilities;
                    color = board.WhiteQueens.IsSet(position) ? Color.White : Color.Black;
                    break;
                default: 
                    throw new ArgumentException();
            }

            BaseBitboard playerPieces = board.GetPlayerPieces(color);
            BaseBitboard opponentPieces = board.GetOpponentPieces(color);

            foreach (var possibility in possibilities)
            {
                int candidate = position + possibility;

                while (candidate >= 0 && candidate < 128 && (candidate & 0x88) == 0)
                {
                    if (playerPieces.IsSet((byte)candidate))
                        break;

                    if (opponentPieces.IsSet((byte) candidate))
                    {
                        yield return board.MakeMove(piece, position, (byte)candidate);
                        break;
                    }
                    
                    yield return board.MakeMove(piece, position, (byte)candidate);
                    candidate = candidate + possibility;
                }
            }
        }
    }
}