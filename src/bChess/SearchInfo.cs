using System;
using System.Collections.Generic;

namespace bChess
{
    public class SearchInfo
    {
        public DateTime StartDate { get; private set; }
        public int VisitedNodes { get; set; }
        public int TableHits { get; set; }
        public List<ChessBoard> BestMoves { get; set; }
        public int Score { get; set; }
        public int NPS => (int) (VisitedNodes / ((DateTime.Now - StartDate).TotalMilliseconds / 1000.0));
        public int Time => (DateTime.Now - StartDate).Milliseconds;

        public SearchInfo() 
        { 
            Reset();
        }

        public SearchInfo(SearchInfo searchInfo)
        {
            StartDate = searchInfo.StartDate;
            VisitedNodes = searchInfo.VisitedNodes;
            TableHits = searchInfo.TableHits;
            BestMoves = new List<ChessBoard>();

            foreach (var move in searchInfo.BestMoves)
                BestMoves.Add(move);

            Score = searchInfo.Score;
        }

        public void Reset()
        {
            StartDate = DateTime.Now;
            VisitedNodes = 0;
            TableHits = 0;
            BestMoves = new List<ChessBoard>();
            Score = int.MinValue + 1;
        }
    }
}