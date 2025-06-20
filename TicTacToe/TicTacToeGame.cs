namespace TicTacToe;

public class TicTacToeGame
{
    /// <summary>
    ///     Array to store the current game state.
    /// </summary>
    private readonly GridEntry[] _grid = Enumerable.Repeat(GridEntry.None, 9).ToArray();

    public GridEntry ActivePlayer { get; private set; } = GridEntry.X;
    public Winner GameState { get; private set; } = Winner.None;

    /// <summary>
    ///     2D indexer for accessing the <see cref="_grid" />
    /// </summary>
    /// <param name="row">The row to read from/write to</param>
    /// <param name="column">The column to read from/write to</param>
    public GridEntry this[int row, int column]
    {
        get => _grid[3 * row + column];
        private set => _grid[3 * row + column] = value;
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

        if (this[row, column] != GridEntry.None)
        {
            throw new ArgumentException("Cell is occupied.");
        }

        this[row, column] = ActivePlayer;
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
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            GameState = returnIfEqual(this[row, 0], this[row, 1], this[row, 2]);

            if (GameState != Winner.None)
            {
                return;
            }
        }
        
        // Check columns
        for (int column = 0; column < 3; column++)
        {
            GameState = returnIfEqual(this[0, column], this[1, column], this[2, column]);

            if (GameState != Winner.None)
            {
                return;
            }
        }
        
        // Check diagonal
        GameState = returnIfEqual(this[0, 0], this[1, 1], this[2, 2]);
        if (GameState != Winner.None)
        {
            return;
        }
        
        GameState = returnIfEqual(this[0, 2], this[1, 1], this[2, 0]);
        if (GameState != Winner.None)
        {
            return;
        }
        
        // Check for draw
        if (!_grid.Contains(GridEntry.None))
        {
            GameState = Winner.Draw;
        }
    }

    private Winner returnIfEqual(GridEntry a, GridEntry b, GridEntry c)
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