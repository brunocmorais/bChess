namespace bChess
{
    public static class Constants
    {
        public const int BoardSize = 64;
        public const ulong WhitePawnStartPosition = 0xFF << 8;
        public const ulong WhiteKnightStartPosition = 0x42;
        public const ulong WhiteBishopStartPosition = 0x24;
        public const ulong WhiteRookStartPosition = 0x81;
        public const ulong WhiteQueenStartPosition = 0x10;
        public const ulong WhiteKingStartPosition = 0x08;
        public const ulong BlackPawnStartPosition = WhitePawnStartPosition << 40;
        public const ulong BlackKnightStartPosition = WhiteKnightStartPosition << 56;
        public const ulong BlackBishopStartPosition = WhiteBishopStartPosition << 56;
        public const ulong BlackRookStartPosition = WhiteRookStartPosition << 56;
        public const ulong BlackQueenStartPosition = WhiteQueenStartPosition << 56;
        public const ulong BlackKingStartPosition = WhiteKingStartPosition << 56;
        public const ulong ColumnMask = 0x101010101010101;
        public const ulong LineMask = 0xFF;
        public static readonly int[] KnightPossibilities = new int[] { 0xE, 0x1F, 0x21, 0x12, -0xE, -0x1F, -0x21, -0x12 };
        public static readonly int[] KingPossibilities = new int[] { 0xF, 0x10, 0x11, 0x1, -0xF, -0x10, -0x11, -0x1 };
        public static readonly int[] BishopPossibilities = new int[] { 0xF, 0x11, -0xF, -0x11 };
        public static readonly int[] RookPossibilities = new int[] { 0x10, 0x1, -0x10, -0x1 };
        public static readonly int[] QueenPossibilities = new int[] {  0xF, 0x10, 0x11, 0x1, -0xF, -0x10, -0x11, -0x1 };
        public const ulong WhiteKingsideCastleMask = 0x6ul;
        public const ulong WhiteQueensideCastleMask = 0x70ul;
        public const ulong BlackKingsideCastleMask = WhiteKingsideCastleMask << 56;
        public const ulong BlackQueensideCastleMask = WhiteQueensideCastleMask << 56;

        public static readonly byte[] Positions = new byte[]
        {
            0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00,
            0x17, 0x16, 0x15, 0x14, 0x13, 0x12, 0x11, 0x10,
            0x27, 0x26, 0x25, 0x24, 0x23, 0x22, 0x21, 0x20,
            0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30,
            0x47, 0x46, 0x45, 0x44, 0x43, 0x42, 0x41, 0x40,
            0x57, 0x56, 0x55, 0x54, 0x53, 0x52, 0x51, 0x50,
            0x67, 0x66, 0x65, 0x64, 0x63, 0x62, 0x61, 0x60,
            0x77, 0x76, 0x75, 0x74, 0x73, 0x72, 0x71, 0x70
        };
        public const int Pawn = 100;
        public const int Knight = 320;
        public const int Bishop = 330;
        public const int Rook = 500;
        public const int Queen = 900;
        public const int King = 20000;

        public static readonly int[] WhitePawnPositions = new int[]
        {
            0,  0,  0,  0,  0,  0,  0,  0,
            5, 10, 10,-20,-20, 10, 10,  5,
            5, -5,-10,  0,  0,-10, -5,  5,
            0,  0,  0, 20, 20,  0,  0,  0,
            5,  5, 10, 25, 25, 10,  5,  5,
            10, 10, 20, 30, 30, 20, 10, 10,
            50, 50, 50, 50, 50, 50, 50, 50,
            0,  0,  0,  0,  0,  0,  0,  0
        };

        public static readonly int[] BlackPawnPositions = new int[]
        {
             0,  0,  0,  0,  0,  0,  0,  0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
             5,  5, 10, 25, 25, 10,  5,  5,
             0,  0,  0, 20, 20,  0,  0,  0,
             5, -5,-10,  0,  0,-10, -5,  5,
             5, 10, 10,-20,-20, 10, 10,  5,
             0,  0,  0,  0,  0,  0,  0,  0
        };

        public static readonly int[] WhiteKnightPositions = new int[]
        {
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50
        };

        public static readonly int[] BlackKnightPositions = new int[]
        {
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50
        };

        public static readonly int[] WhiteBishopPositions = new int[]
        {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -20,-10,-10,-10,-10,-10,-10,-20
        };

        public static readonly int[] BlackBishopPositions = new int[]
        {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20
        };

        public static readonly int[] WhiteRookPositions = new int[]
        {
            0,  0,  0,  5,  5,  0,  0,  0,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            5, 10, 10, 10, 10, 10, 10,  5,
            0,  0,  0,  0,  0,  0,  0,  0
        };

        public static readonly int[] BlackRookPositions = new int[]
        {
            0,  0,  0,  0,  0,  0,  0,  0,
            5, 10, 10, 10, 10, 10, 10,  5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            0,  0,  0,  5,  5,  0,  0,  0
        };

        public static readonly int[] WhiteQueenPositions = new int[]
        {
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  5,  5,  5,  5,  5,  0,-10,
            0,  0,  5,  5,  5,  5,  0, -5,
            -5,  0,  5,  5,  5,  5,  0, -5,
            -10,  0,  5,  5,  5,  5,  0,-10,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -20,-10,-10, -5, -5,-10,-10,-20
        };

        public static readonly int[] BlackQueenPositions = new int[]
        {
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5,  5,  5,  5,  0,-10,
             -5,  0,  5,  5,  5,  5,  0, -5,
              0,  0,  5,  5,  5,  5,  0, -5,
            -10,  5,  5,  5,  5,  5,  0,-10,
            -10,  0,  5,  0,  0,  0,  0,-10,
            -20,-10,-10, -5, -5,-10,-10,-20
        };

        public static readonly int[] WhiteKingPositions = new int[]
        {
            20, 30, 10,  0,  0, 10, 30, 20,
            20, 20,  0,  0,  0,  0, 20, 20,
            -10,-20,-20,-20,-20,-20,-20,-10,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30
        };

        public static readonly int[] BlackKingPositions = new int[]
        {
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
            20, 20,  0,  0,  0,  0, 20, 20,
            20, 30, 10,  0,  0, 10, 30, 20
        };
    }
}