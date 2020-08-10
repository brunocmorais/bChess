using System;
using System.Collections.Generic;
using System.Linq;

namespace bChess
{
    public class SearchInfo
    {
        public DateTime StartDate { get; private set; }
        public int VisitedNodes { get; set; }
        public int TableHits { get; set; }
        public IEnumerable<ChessBoard> BestMoves => PossibleMoves.Where(x => x.Value == MaxScore).Select(x => x.Key);
        public int MaxScore => PossibleMoves.Max(x => x.Value);
        public int NPS => (int) (VisitedNodes / ((DateTime.Now - StartDate).TotalMilliseconds / 1000.0));
        public int Time => (DateTime.Now - StartDate).Milliseconds;
        public Dictionary<ChessBoard, int> PossibleMoves { get; private set; }
        public int Depth { get; set; }

        public SearchInfo() 
        { 
            Reset();
        }

        public SearchInfo(SearchInfo searchInfo)
        {
            StartDate = searchInfo.StartDate;
            VisitedNodes = searchInfo.VisitedNodes;
            TableHits = searchInfo.TableHits;
            Depth = searchInfo.Depth;
            PossibleMoves = new Dictionary<ChessBoard, int>();

            foreach (var entry in searchInfo.PossibleMoves)
                PossibleMoves.Add(entry.Key, entry.Value);
        }

        public void Reset()
        {
            StartDate = DateTime.Now;
            VisitedNodes = 0;
            TableHits = 0;
            Depth = 0;
            PossibleMoves = new Dictionary<ChessBoard, int>();
        }
    }
}