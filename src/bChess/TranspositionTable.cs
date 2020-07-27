namespace bChess
{
    public static class TranspositionTable
    {
        private static TranspositionTableInfo[] Array = new TranspositionTableInfo[0x100000];

        public static TranspositionTableInfo Find(ChessBoard board)
        {
            TranspositionTableInfo hashInformation = Array[board.Hash % (ulong) Array.Length];

            if (board.NextTurn != hashInformation.Color)
                hashInformation.Value = -hashInformation.Value;

            return hashInformation;
        }

        public static void Add(ChessBoard board, int depth, int value, Transposition type)
        {
            Array[board.Hash % (ulong) Array.Length] = new TranspositionTableInfo(board.Hash, depth, value, board.NextTurn, type);
        }
    }
}