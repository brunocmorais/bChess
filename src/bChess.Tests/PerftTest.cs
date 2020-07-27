using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bChess.Tests
{
    [TestClass]
    public class PerftTest
    {
        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 20)]
        [DataRow(2, 400)]
        [DataRow(3, 8902)]
        [DataRow(4, 197281)]
        public void Test(int ply, int expected)
        {
            var chessBoard = new ChessBoard();
            int actual = Perft.DoPerft(chessBoard, ply);
            Assert.AreEqual(expected, actual);
        }
    }
}
