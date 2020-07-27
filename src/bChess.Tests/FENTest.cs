using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bChess.Tests
{
    [TestClass]
    public class FENTest
    {
        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Property)]
        public void Write(ChessBoard chessBoard, string expectedFen)
        {
            string fen = FEN.Write(chessBoard);
            Assert.AreEqual(expectedFen, fen);
        }

        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Property)]
        public void Read(ChessBoard expectedBoard, string fen)
        {
            ChessBoard board = FEN.Read(fen);
            Assert.AreEqual(expectedBoard, board);
        }

        static IEnumerable<object[]> Data
        {
            get
            {
                var chessBoard = new ChessBoard();
                yield return new object[] { chessBoard, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" };
                
                chessBoard = chessBoard.MakeMove(Piece.Pawn, 0x14, 0x34);
                yield return new object[] { chessBoard, "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1" };

                chessBoard = chessBoard.MakeMove(Piece.Pawn, 0x62, 0x42);
                yield return new object[] { chessBoard, "rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1" };
            }
        }
    }
}
