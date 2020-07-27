using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bChess
{
    public static class Search
    {
        // https://medium.com/@andreasstckl/writing-a-chess-program-in-one-day-30daff4610ec
        public static ChessBoard SelectMove(ChessBoard board, int depth)
        {
            List<ChessBoard> bestMoves = new List<ChessBoard>();
            var bestValue = int.MinValue + 1;
            var moves = MoveGenerator.GetAllMovesForPosition(board);
            moves = OrderMoves(moves);

            foreach (var move in moves)
            {
                int? boardValue = -AlphaBeta(move, int.MinValue, int.MaxValue, depth, move.NextTurn == Color.White ? 1 : -1);
                
                if (boardValue == null) // movimento inválido
                    continue;

                if (boardValue > bestValue) // novo melhor movimento, limpa os anteriores
                {
                    bestValue = boardValue.Value;
                    bestMoves.Clear();
                    bestMoves.Add(move);
                }
                else if (boardValue == bestValue) // movimento tão bom quanto o já encontrado, adicionar para ter mais opções 
                    bestMoves.Add(move);
            }

            return bestMoves[new Random().Next(0, bestMoves.Count)];
        }

        private static int? AlphaBeta(ChessBoard board, int alpha, int beta, int depth, int color)
        {
            int originalAlpha = alpha;
            TranspositionTableInfo ttInfo = TranspositionTable.Find(board);
            
            if (ttInfo.Hash == board.Hash && ttInfo.Depth >= depth)
            {
                switch (ttInfo.Type)
                {
                    case Transposition.Exact: return ttInfo.Value;
                    case Transposition.Lower: alpha = Math.Max(alpha, ttInfo.Value); break;
                    case Transposition.Upper: beta  = Math.Min(beta, ttInfo.Value); break;
                }

                if (alpha >= beta)
                    return ttInfo.Value;
            }

            if (depth == 0)
                return Evaluator.Evaluate(board) * color;

            var moves = MoveGenerator.GetAllMovesForPosition(board);

            if (moves.Any(x => x == null)) // movimento inválido
                return null;

            moves = OrderMoves(moves);
            int value = int.MinValue + 1;

            foreach (var move in moves)
            {
                int? score = -AlphaBeta(move, -beta, -alpha, depth - 1, -color);

                if (score == null) // movimento inválido
                    continue;

                value = Math.Max(value, score.Value);
                alpha = Math.Max(alpha, value);
                
                if (alpha >= beta)
                    break;
            }

            Transposition type;

            if (value <= originalAlpha)
                type = Transposition.Upper;
            else if (value >= beta)
                type = Transposition.Lower;
            else
                type = Transposition.Exact;

            TranspositionTable.Add(board, depth, value, type);

            return value;
        }

        private static IEnumerable<ChessBoard> OrderMoves(IEnumerable<ChessBoard> moves)
        {
            return moves.OrderByDescending(x => x.CaptureInfo.GetCaptureDelta());
        }
    }
}