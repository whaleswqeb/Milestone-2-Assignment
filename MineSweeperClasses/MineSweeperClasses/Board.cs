
using System;

namespace MineSweeperClasses
{
    public class Board
    {
        public Cell[,] grid { get; private set; }
        public bool RewardAvailable { get; set; }
        public bool HasUsedReward { get; set; }

        private int size;
        private float bombProbability;

        public Board(int size, float bombProbability)
        {
            this.size = size;
            this.bombProbability = bombProbability;
            grid = new Cell[size, size];
            RewardAvailable = false;
            HasUsedReward = false;

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Random rand = new Random();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    grid[row, col] = new Cell();

                    if (rand.NextDouble() < bombProbability)
                        grid[row, col].HasBomb = true;
                }
            }

            // Place one reward on a safe cell
            int rewardRow, rewardCol;
            do
            {
                rewardRow = rand.Next(size);
                rewardCol = rand.Next(size);
            } while (grid[rewardRow, rewardCol].HasBomb);

            grid[rewardRow, rewardCol].HasReward = true;
        }

        // DetermineGameState updated to check flags and visited cells correctly
        public GameState DetermineGameState()
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var cell = grid[row, col];

                    // If a bomb cell has been visited, player loses
                    if (cell.HasBomb && cell.IsVisited)
                        return GameState.Lost;

                    // If a safe cell is neither visited nor flagged correctly, game still in progress
                    if (!cell.HasBomb && !cell.IsVisited)
                        return GameState.StillPlaying;

                    // If a bomb cell is not flagged, and game might not be won yet
                    if (cell.HasBomb && !cell.IsFlagged)
                        return GameState.StillPlaying;
                }
            }

            // If all safe cells visited and bombs flagged, player wins
            return GameState.Won;
        }
        public static void PrintBoard(Board board)
        {
            int size = board.grid.GetLength(0);

            // Print column headers
            Console.Write("    ");  // indent for row numbers
            for (int col = 0; col < size; col++)
            {
                Console.Write($" {col}  ");
            }
            Console.WriteLine();

            // Print top border
            Console.Write("  +");
            for (int col = 0; col < size; col++)
            {
                Console.Write("---+");
            }
            Console.WriteLine();

            // Print each row with cell contents and vertical borders
            for (int row = 0; row < size; row++)
            {
                Console.Write($"{row} |");

                for (int col = 0; col < size; col++)
                {
                    Cell cell = board.grid[row, col];

                    if (cell.IsFlagged)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; // Flag color = Cyan
                        Console.Write(" F ");
                    }
                    else if (!cell.IsVisited)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue; // Unvisited = Blue '?'
                        Console.Write(" ? ");
                    }
                    else if (cell.HasBomb)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Bomb = Red 'B'
                        Console.Write(" B ");
                    }
                    else if (cell.HasReward)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta; // Reward = Purple 'r'
                        Console.Write(" r ");
                    }
                    else
                    {
                        // Safe visited cells show "0" in White
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" 0 ");
                    }

                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.WriteLine();

                // Print row separator line
                Console.Write("  +");
                for (int col = 0; col < size; col++)
                {
                    Console.Write("---+");
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        // PrintBoard with visited and flagged check, no background color, custom colors for digits
        //public static void PrintBoard(Board board)
        //{
        //    int size = board.grid.GetLength(0);

        //    Console.Write("   ");
        //    for (int i = 0; i < size; i++)
        //        Console.Write(i + " ");
        //    Console.WriteLine();

        //    Console.Write("  ");
        //    for (int i = 0; i < size * 2 + 1; i++)
        //        Console.Write("-");
        //    Console.WriteLine();

        //    for (int row = 0; row < size; row++)
        //    {
        //        Console.Write(row + " | ");
        //        for (int col = 0; col < size; col++)
        //        {
        //            Cell cell = board.grid[row, col];

        //            if (cell.IsFlagged)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Cyan; // Flag color = Cyan
        //                Console.Write("F ");
        //            }
        //            else if (!cell.IsVisited)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Blue; // Unvisited = Blue '?'
        //                Console.Write("? ");
        //            }
        //            else if (cell.HasBomb)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Red; // Bomb = Red 'B'
        //                Console.Write("B ");
        //            }
        //            else if (cell.HasReward)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Magenta; // Reward = Purple 'r'
        //                Console.Write("r ");
        //            }
        //            else
        //            {
        //                // Safe visited cells show "0" in White
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.Write("0 ");
        //            }

        //            Console.ResetColor();
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.ResetColor();
        //}

        // PrintAnswers always show full board, for cheating/debug
        //public void PrintAnswers()
        //{
        //    int size = grid.GetLength(0);

        //    Console.Write("   ");
        //    for (int i = 0; i < size; i++)
        //        Console.Write(i + " ");
        //    Console.WriteLine();

        //    Console.Write("  ");
        //    for (int i = 0; i < size * 2 + 1; i++)
        //        Console.Write("-");
        //    Console.WriteLine();

        //    for (int row = 0; row < size; row++)
        //    {
        //        Console.Write(row + " | ");
        //        for (int col = 0; col < size; col++)
        //        {
        //            if (grid[row, col].HasBomb)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                Console.Write("B ");
        //            }
        //            else if (grid[row, col].HasReward)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Magenta;
        //                Console.Write("r ");
        //            }
        //            else
        //            {
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.Write("0 ");
        //            }
        //            Console.ResetColor();
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.ResetColor();
        //}
        public void PrintAnswers()
        {
            int size = grid.GetLength(0);

            // Print column headers
            Console.Write("    ");  // indent for row numbers
            for (int col = 0; col < size; col++)
            {
                Console.Write($" {col}  ");
            }
            Console.WriteLine();

            // Print top border
            Console.Write("  +");
            for (int col = 0; col < size; col++)
            {
                Console.Write("---+");
            }
            Console.WriteLine();

            // Print each row
            for (int row = 0; row < size; row++)
            {
                Console.Write($"{row} |"); // row number

                for (int col = 0; col < size; col++)
                {
                    if (grid[row, col].HasBomb)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" B ");
                    }
                    else if (grid[row, col].HasReward)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(" r ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" 0 ");
                    }
                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.WriteLine();

                // Print row separator line
                Console.Write("  +");
                for (int col = 0; col < size; col++)
                {
                    Console.Write("---+");
                }
                Console.WriteLine();
            }
        }

    }

    public enum GameState
    {
        StillPlaying,
        Won,
        Lost
    }
}
