using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bChess.Tests
{
    [TestClass]
    public class SpecialMoveTest
    {
        [TestMethod]
        public void PromotionTest()
        {
            var chessBoard = FEN.Read("4k3/P7/8/8/8/8/8/4K3 w - - 0 1");
            chessBoard = chessBoard.MakeMove(Piece.Pawn, 0x60, 0x70);
        }

        [TestMethod]
        public void CastlingTest()
        {
            var chessBoard = FEN.Read("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b KQkq - 0 1");
            var listMoves = MoveGenerator.GenerateMoves(chessBoard, 0x74).ToList();
        }
    }
}
