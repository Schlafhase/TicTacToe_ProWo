using System.Text;
using TicTacToe;

namespace TicTacToeWithAI;

public static class TicTacToeAI
{
    public static (int row, int col) GetBestMove(GridEntry[][] board, GridEntry aiSymbol)
    {
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row][col] != GridEntry.None)
                {
                    continue;
                }

                board[row][col] = aiSymbol;
                int score = miniMax(board, aiSymbol);

                Console.WriteLine(GetAsString(board));
                Console.WriteLine(score);
                board[row][col] = GridEntry.None;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = (row, col);
                    Console.WriteLine($"AI: Found new best move: {bestMove.row}, {bestMove.col} with score {score}");
                }
            }
        }

        return bestMove;
    }

    public static string GetAsString(GridEntry[][] board)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("  1 2 3");
        sb.AppendLine();

        for (int row = 0; row < 3; row++)
        {
            sb.Append($"{row + 1} ");
            for (int col = 0; col < 3; col++)
            {
                GridEntry cell = board[row][col];
                string value = cell switch
                {
                    GridEntry.X => "X ",
                    GridEntry.O => "O ",
                    _ => "  "
                };
                sb.Append(value);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static int miniMax(GridEntry[][] board, GridEntry aiSymbol, int depth = 0,
        bool maximising = false)
    {
        Winner winner = TicTacToeGame.CheckForWin(board);

        if (winner != Winner.None)
        {
            if (winner == Winner.Draw)
            {
                return 0;
            }

            if (winner.ToString() == aiSymbol.ToString())
            {
                // High score = AI has advantage
                return 10 - depth;
            }

            // Low score = Human has advantage
            return depth - 10;
        }

        if (maximising)
        {
            // Player is AI -> wants to get high score
            int bestScore = int.MinValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row][col] != GridEntry.None)
                    {
                        continue;
                    }

                    board[row][col] = aiSymbol;
                    int score = miniMax(board, aiSymbol, depth + 1, false);
                    board[row][col] = GridEntry.None;
                    bestScore = Math.Max(bestScore, score); // Higher score = better
                }
            }


            return bestScore;
        }
        else
        {
            // Player is Human -> wants to get low score (better for human)
            int bestScore = int.MaxValue;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row][col] != GridEntry.None)
                    {
                        continue;
                    }

                    board[row][col] = otherSymbol(aiSymbol);
                    int score = miniMax(board, aiSymbol, depth + 1, true);
                    board[row][col] = GridEntry.None;
                    bestScore = Math.Min(bestScore, score); // Lower score = better
                }
            }

            return bestScore;
        }
    }

    private static GridEntry otherSymbol(GridEntry symbol)
    {
        return symbol switch
        {
            GridEntry.O => GridEntry.X,
            GridEntry.X => GridEntry.O,
            _ => GridEntry.None,
        };
    }
}