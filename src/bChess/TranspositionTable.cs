using System;

namespace bChess
{
    public static class TranspositionTable
    {
        private static TranspositionTableInfo[] Table = new TranspositionTableInfo[0x100000];

        public static TranspositionTableInfo Find(ChessBoard board)
        {
            TranspositionTableInfo hashInformation = Table[board.Hash % (ulong) Table.Length];

            if (board.NextTurn != hashInformation.Color)
                hashInformation.Value = -hashInformation.Value;

            return hashInformation;
        }

        public static void Add(ChessBoard board, int depth, int value, Transposition type)
        {
            Table[board.Hash % (ulong) Table.Length] = new TranspositionTableInfo(board.Hash, depth, value, board.NextTurn, type);
        }

        public static void Clear()
        {
            Array.Clear(Table, 0, Table.Length);
        }
    }
}