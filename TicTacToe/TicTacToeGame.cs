using System.Text;

namespace TicTacToe;

public class TicTacToeGame
{
    /// <summary>
    ///     Array to store the current game state.
    /// </summary>
    private readonly GridEntry[][] _grid;

    public GridEntry ActivePlayer { get; private set; }
    public Winner GameState { get; private set; } = Winner.None;

    private readonly Random _random = new Random();
    public GridEntry[][] Grid => _grid;

    public TicTacToeGame()
    {
        ActivePlayer = _random.Next(2) == 0 ? GridEntry.O : GridEntry.X;

        _grid = new GridEntry[3][];

        for (int row = 0; row < 3; row++)
        {
            _grid[row] = new GridEntry[3];
            for (int col = 0; col < 3; col++)
            {
                _grid[row][col] = GridEntry.None;
            }
        }
    }

    /// <summary>
    ///     This method will execute a move for the <see cref="ActivePlayer" />
    /// </summary>
    /// <param name="row">Target row</param>
    /// <param name="column">Target column</param>
    /// <exception cref="ArgumentException">
    ///     Throws when row or column are out of range or
    ///     when the cell in the grid is already occupied.
    /// </exception>
    public void Move(int row, int column)
    {
        if (GameState != Winner.None)
        {
            throw new InvalidOperationException("Game has ended");
        }

        if (row > 2 || column > 2 || row < 0 || column < 0)
        {
            throw new ArgumentException("Row and Column are out of range.");
        }

        if (_grid[row][column] != GridEntry.None)
        {
            throw new ArgumentException("Cell is occupied.");
        }

        _grid[row][column] = ActivePlayer;
        toggleActivePlayer();
        checkForWin();
    }

    private void toggleActivePlayer()
    {
        ActivePlayer = ActivePlayer == GridEntry.X ? GridEntry.O : GridEntry.X;
    }

    /// <summary>
    /// Checks whether a player has won
    /// </summary>
    private void checkForWin()
    {
        GameState = CheckForWin(_grid);
    }

    public static Winner CheckForWin(GridEntry[][] board)
    {
        Winner winner;

        // Check rows
        for (int row = 0; row < 3; row++)
        {
            winner = returnIfEqual(board[row][0], board[row][1], board[row][2]);

            if (winner != Winner.None)
            {
                return winner;
            }
        }

        // Check columns
        for (int column = 0; column < 3; column++)
        {
            winner = returnIfEqual(board[0][column], board[1][column], board[2][column]);

            if (winner != Winner.None)
            {
                return winner;
            }
        }

        // Check diagonal
        winner = returnIfEqual(board[0][0], board[1][1], board[2][2]);
        if (winner != Winner.None)
        {
            return winner;
        }

        winner = returnIfEqual(board[0][2], board[1][1], board[2][0]);
        if (winner != Winner.None)
        {
            return winner;
        }

        // Check for draw
        bool hasEmptyCells = false;

        for (int row = 0; row < 3; row++)
        {
            if (board[row].Contains(GridEntry.None))
            {
                hasEmptyCells = true;
                break;
            }
        }

        return hasEmptyCells ? Winner.None : Winner.Draw;
    }

    private static Winner returnIfEqual(GridEntry a, GridEntry b, GridEntry c)
    {
        if (a == b && b == c)
        {
            return a switch
            {
                GridEntry.None => Winner.None,
                GridEntry.X => Winner.X,
                GridEntry.O => Winner.O,
                _ => throw new ArgumentOutOfRangeException(nameof(a), a, null)
            };
        }

        return Winner.None;
    }

    public string GetAsString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("  1 2 3");
        sb.AppendLine();

        for (int row = 0; row < 3; row++)
        {
            sb.Append($"{row + 1} ");
            for (int col = 0; col < 3; col++)
            {
                GridEntry cell = _grid[row][col];
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
}

public enum GridEntry
{
    None,
    X,
    O
}

public enum Winner
{
    None,
    X,
    O,
    Draw
}