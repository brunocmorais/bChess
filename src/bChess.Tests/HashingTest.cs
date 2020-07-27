using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bChess.Tests
{
    [TestClass]
    public class HashingTest
    {
        [TestMethod]
        public void UpdateHashForPromotionTest()
        {
            var chessBoard = FEN.Read("4k3/P7/8/8/8/8/8/4K3 w - - 0 1");
            chessBoard = chessBoard.MakeMove(Piece.Pawn, 0x60, 0x70, Piece.Queen);
            var promotedBoard = FEN.Read("Q3k3/8/8/8/8/8/8/4K3 w - - 0 1");
            Assert.AreEqual(chessBoard.Hash, promotedBoard.Hash);
        }
    }
}
