namespace bChess
{
    public static class Perft
    {
        public static int DoPerft(ChessBoard board, int ply)
        {
            if (board.WhiteKing == 0 || board.BlackKing == 0)
                return 0;

            if (ply == 0)
                return 1;

            var moves = MoveGenerator.GetAllMovesForPosition(board);
            int sum = 0;

            foreach (var move in moves)
            {
                int currentSum = DoPerft(move, ply - 1);

                if (currentSum == 0)
                    continue;

                sum += currentSum; 
                Evaluator.Evaluate(move);
            }

            return sum;
        }
    }
}