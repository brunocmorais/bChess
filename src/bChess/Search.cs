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
        private static List<ChessBoard> moveHistory;

        public static ChessBoard Start(ChessBoard board, int maxDepth, List<ChessBoard> moveHistory, CancellationToken ct)
        {
            Search.moveHistory = moveHistory;
            searchInfo.Reset();
            cancellationToken = ct;

            for (int depth = 1; depth <= maxDepth; depth++)
            {
                searchInfo.Depth = depth;
                List<ChessBoard> moves;

                if (searchInfo.PossibleMoves.Any())
                    moves = searchInfo.PossibleMoves.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
                else
                {
                    moves = MoveGenerator.GetPseudoMovesForPosition(board).ToList();

                    foreach (var move in moves)
                        searchInfo.PossibleMoves.Add(move, 0);
                }

                if (moves.Count >= 2) // efetua busca com duas threads para ganhar velocidade
                {
                    var intervals = new[] { 0, moves.Count / 2, moves.Count };
                    var tasks = new Task[2];

                    tasks[0] = Task.Run(() => ProcessInnerTree(moves.GetRange(intervals[0], intervals[1] - intervals[0]), depth), ct);
                    tasks[1] = Task.Run(() => ProcessInnerTree(moves.GetRange(intervals[1], intervals[2] - intervals[1]), depth), ct);
                    
                    Task.WaitAll(tasks);
                }
                else
                    ProcessInnerTree(moves, depth);

            }
            
            return searchInfo.BestMoves.ToList()[new Random().Next(0, searchInfo.BestMoves.Count())];
        }

        private static void ProcessInnerTree(IEnumerable<ChessBoard> moves, int depth)
        {
            foreach (var move in moves)
            {
                if (moveHistory.Count(x => x.Hash == move.Hash) == 2)
                {
                    searchInfo.PossibleMoves[move] = 0;
                    continue;
                }

                int? boardValue = -AlphaBeta(move, int.MinValue, int.MaxValue, depth, move.NextTurn == Color.White ? 1 : -1);

                if (!boardValue.HasValue) // movimento inválido
                    searchInfo.PossibleMoves[move] = int.MinValue;
                else
                    searchInfo.PossibleMoves[move] = boardValue.Value;
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
            {
                int evaluate = Evaluator.Evaluate(board) * color;
                //TranspositionTable.Add(board, depth, evaluate, Transposition.Exact);
                return evaluate;
            }

            var moves = MoveGenerator.GetPseudoMovesForPosition(board);

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

            if (value < -Constants.AllPiecesSum && !Evaluator.IsInCheck(board)) // rei afogado
                value = 0;

            TranspositionTable.Add(board, depth, value, type);

            return value;
        }

        public static int? Quiesce(ChessBoard board, int alpha, int beta, int color)
        {
            searchInfo.VisitedNodes++;

            TranspositionTableInfo ttInfo = TranspositionTable.Find(board);
            
            if (ttInfo.Hash == board.Hash && ttInfo.Type == Transposition.Exact)
            {
                searchInfo.TableHits++;
                return ttInfo.Value;
            }

            var evaluate = Evaluator.Evaluate(board) * color;

            TranspositionTable.Add(board, 0, evaluate, Transposition.Exact);

            if (evaluate >= beta)
                return beta;
            
            if (alpha < evaluate)
                alpha = evaluate;

            var moves = MoveGenerator.GetPseudoMovesForPosition(board);

            if (moves.Any(x => x == null))
                return null;

            foreach (var move in moves)
            {
                if (move.CaptureInfo.IsCapture)
                {
                    int? score = -Quiesce(move, -beta, -alpha, -color);

                    if (score == null) // movimento inválido
                        continue;

                    if (score >= beta)
                        return beta;
                    if (score > alpha)
                        alpha = score.Value;
                }
            }

            return alpha;
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