using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bChess
{
    public static class Search
    {
        private static readonly SearchInfo searchInfo = new SearchInfo();
        private static CancellationToken cancellationToken;

        public static ChessBoard Start(ChessBoard board, int depth, CancellationToken ct)
        {
            searchInfo.Reset();

            cancellationToken = ct;
            var moves = OrderMoves(MoveGenerator.GetAllMovesForPosition(board)).ToList();
            
            searchInfo.BestMoves.Add(moves.First());

            if (moves.Count >= 2)
            {
                var intervals = new[] { 0, moves.Count / 2, moves.Count };
                var tasks = new Task[2];

                tasks[0] = Task.Run(() => ProcessInnerTree(moves.GetRange(intervals[0], intervals[1] - intervals[0]), depth), ct);
                tasks[1] = Task.Run(() => ProcessInnerTree(moves.GetRange(intervals[1], intervals[2] - intervals[1]), depth), ct);
                
                Task.WaitAll(tasks);
            }
            else
                ProcessInnerTree(moves, depth);

            // VERIFICAR PORQUE NÃO ESTÁ MAIS VOLTANDO O BESTMOVE APÓS O STOP

            return searchInfo.BestMoves[new Random().Next(0, searchInfo.BestMoves.Count)];
        }

        private static void ProcessInnerTree(IEnumerable<ChessBoard> moves, int depth)
        {
            foreach (var move in moves)
            {
                int? boardValue = -AlphaBeta(move, int.MinValue, int.MaxValue, depth, move.NextTurn == Color.White ? 1 : -1);

                if (boardValue != null) // movimento inválido
                {
                    if (boardValue > searchInfo.Score) // novo melhor movimento, limpa os anteriores
                    {
                        searchInfo.Score = boardValue.Value;
                        searchInfo.BestMoves.Clear();
                        searchInfo.BestMoves.Add(move);
                        searchInfo.Score = boardValue.Value;
                    }
                    else if (boardValue == searchInfo.Score) // movimento tão bom quanto o já encontrado, adicionar para ter mais opções 
                        searchInfo.BestMoves.Add(move);
                }
            }
        }

        private static int? AlphaBeta(ChessBoard board, int alpha, int beta, int depth, int color)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            searchInfo.VisitedNodes++;

            int originalAlpha = alpha;
            TranspositionTableInfo ttInfo = TranspositionTable.Find(board);
            
            if (ttInfo.Hash == board.Hash && ttInfo.Depth >= depth)
            {
                searchInfo.TableHits++;

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

        public static SearchInfo GetSearchInfo()
        {
            SearchInfo searchInfoCopy;

            lock (searchInfo)
                searchInfoCopy = new SearchInfo(searchInfo);

            return searchInfoCopy;
        }
    }
}