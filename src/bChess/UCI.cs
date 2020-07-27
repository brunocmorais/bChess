using System;

namespace bChess
{
    public static class UCI
    {
        private static ChessBoard board = new ChessBoard();
        private static int Level = 3;

        public static void Init()
        {
            ReadCommands();
        }

        private static void InitBoard()
        {
            board = new ChessBoard();
        }

        private static string GetMove(byte from, byte to)
        {
            return $"{GetMove(from)}{GetMove(to)}";
        }

        private static string GetMove(byte position)
        {
            char first = (char)(97 + ((position & 0xF)));
		    char second = (char)(49 + ((position & 0xF0) >> 4));
            return $"{first}{second}";
        }

        private static byte[] GetPositions(string move)
        {
            string from = move.Substring(0, 2);
            string to = move.Substring(2);

            return new byte[] { GetPosition(from), GetPosition(to) };
        }

        private static byte GetPosition(string position)
        {
            var first = position[0] - 97;
		    var second = position[1] - 49;
		    return (byte) ((second << 4) | first);
        }

        private static void ReadCommands()
        {
            bool exit = false;

            while (!exit)
            {
                string command = Console.ReadLine();

                switch (command.Split(" ")[0])
                {
                    case "uci": PrintInfo(); break;
                    case "isready": WriteResponse("readyok"); break;
                    case "position": ReadPosition(command); break;
                    case "ucinewgame": InitBoard(); break;
                    case "go": Go(); break;
                    case "setoption": SetOption(command); break;
                    case "quit": exit = true; break;
                    default: break;
                }
            }
        }

        private static void SetOption(string command)
        {
            string[] parameters = command.Trim().Split(" ");

            switch (parameters[2])
            {
                case "Level":
                    Level = int.Parse(parameters[4]);
                    break;
            }
        }

        private static void Go()
        {
            board = Search.SelectMove(board, Level);
            WriteResponse($"bestmove {GetMove(board.From, board.To)}");
        }

        private static void ReadPosition(string command)
        {
            int movesIndex = command.LastIndexOf("moves");

            if (command.Contains("startpos"))
                InitBoard();       
            else if (command.Contains("fen"))
            {
                string fen;
                int fenIndex = command.LastIndexOf("fen");

                if (movesIndex > 0)
                    fen = command.Substring(fenIndex + 3, movesIndex - 5);
                else
                    fen = command.Substring(fenIndex + 3);

                board = FEN.Read(fen.Trim());
            }

            if (movesIndex > 0)
            {
                string moves = command.Substring(movesIndex).Replace("moves", string.Empty).Trim();

                string[] moveArray = moves.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                foreach (var move in moveArray)
                {
                    byte[] position = GetPositions(move);
                    byte from = position[0];
                    byte to = position[1];
                    board = board.MakeMove(board.GetPiece(from), /*board.NextTurn, */from, to);
                }
            }
        }

        private static void PrintInfo()
        {
            WriteResponse("uci");
            WriteResponse("id name bChess v2.0");
            WriteResponse("id author Bruno Costa de Morais");
            WriteResponse("option name Level type spin default 3 min 1 max 5");
            WriteResponse("uciok"); 
        }

        private static void WriteResponse(string message)
        {
            Console.WriteLine(message);
        }
    }
}