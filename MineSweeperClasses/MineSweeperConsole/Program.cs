
using System;
using MineSweeperClasses;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Welcome to Minesweeper!");

        int size = 5;                 // You can change the size here
        float bombProbability = 0.2f; // Probability of bomb in each cell

        Board board = new Board(size, bombProbability);

        // Cheat: Show full answer board once at start
        Console.WriteLine("Here is the the answer key for the first board:");
        board.PrintAnswers();

        bool victory = false;
        bool death = false;

        while (!victory && !death)
        {
            Board.PrintBoard(board);

            Console.Write("Enter row (0-" + (size - 1) + "): ");
            int row = GetValidInput(0, size - 1);

            Console.Write("Enter column (0-" + (size - 1) + "): ");
            int col = GetValidInput(0, size - 1);

            Console.WriteLine("Choose action: 1=Flag, 2=Visit, 3=Use Reward");
            int action = GetValidInput(1, 3);

            if (action == 1) // Flag
            {
                board.grid[row, col].IsFlagged = !board.grid[row, col].IsFlagged;
                Console.WriteLine(board.grid[row, col].IsFlagged ? "Flag placed." : "Flag removed.");
            }
            else if (action == 2) // Visit
            {
                if (board.grid[row, col].IsFlagged)
                {
                    Console.WriteLine("Cannot visit a flagged cell! Remove the flag first.");
                    continue;
                }

                if (board.grid[row, col].IsVisited)
                {
                    Console.WriteLine("Cell already visited.");
                    continue;
                }

                board.grid[row, col].IsVisited = true;

                if (board.grid[row, col].HasReward)
                {
                    Console.WriteLine("Congrats! You found a reward! You can use it next turn.");
                    board.RewardAvailable = true;
                    board.HasUsedReward = false;
                }
            }
            else if (action == 3) // Use Reward
            {
                if (!board.RewardAvailable)
                {
                    Console.WriteLine("No reward available to use.");
                    continue;
                }
                if (board.HasUsedReward)
                {
                    Console.WriteLine("You already used the reward this turn.");
                    continue;
                }

                Console.Write("Enter row to peek: ");
                int peekRow = GetValidInput(0, size - 1);

                Console.Write("Enter column to peek: ");
                int peekCol = GetValidInput(0, size - 1);

                bool isBomb = board.grid[peekRow, peekCol].HasBomb;

                Console.WriteLine($"Peeking cell [{peekRow},{peekCol}]: It {(isBomb ? "IS" : "is NOT")} a bomb!");

                board.HasUsedReward = true;
                board.RewardAvailable = false;
            }

            // Determine game state
            var state = board.DetermineGameState();

            if (state == GameState.Won)
                victory = true;
            else if (state == GameState.Lost)
                death = true;
        }

        Board.PrintBoard(board);

        if (victory)
            Console.WriteLine("Congratulations! You won the game!");
        else if (death)
            Console.WriteLine("Oops! You hit a bomb. Game Over.");
    }

    static int GetValidInput(int min, int max)
    {
        int val;
        while (!int.TryParse(Console.ReadLine(), out val) || val < min || val > max)
        {
            Console.Write($"Invalid input. Enter a number between {min} and {max}: ");
        }
        return val;
    }
}
