using System.Text;

namespace TicTacToe;

/// <summary>
///     Repräsentiert einen Eintrag in das Spielbrett
/// </summary>
public enum GridEntry
{
    None, // Leeres Feld
    X,
    O
}

/// <summary>
///     Repräsentiert einen Gewinner
/// </summary>
public enum Winner
{
    None, // Kein Gewinner
    X,
    O,
    Draw // Gleichstand
}

/// <summary>
///     Repräsentiert ein TicTacToe-Spiel
/// </summary>
public class TicTacToeGame
{
    /// <summary>
    ///     Instanz der <see cref="Random" /> Klasse, um Zufalls-Zahlen zu generieren
    /// </summary>
    private readonly Random _random = new();

    /// <summary>
    ///     Konstruktor der Klasse <see cref="TicTacToeGame" />; initialisiert Felder und Eigenschaften der Instanz
    /// </summary>
    public TicTacToeGame()
    {
        // Der beginnende Spieler wird zufällig ausgewählt
        // (Wenn die Zufalls-Zahl gleich null ist, beginnt O, sonst X
        ActivePlayer = _random.Next(2) == 0 ? GridEntry.O : GridEntry.X;

        // Das Spielfeld besteht aus 3 Zeilen.
        Grid = new GridEntry[3][];

        for (int row = 0; row < 3; row++)
        {
            // Jede Zeile hat 3 Elemente (Spalten)
            Grid[row] = new GridEntry[3];
            for (int col = 0; col < 3; col++)
            {
                // Jedes Feld hat den Standardwert GridEntry.None
                Grid[row][col] = GridEntry.None;
            }
        }
    }

    /// <summary>
    ///     Speichert den Spieler, der gerade dran ist
    /// </summary>
    public GridEntry ActivePlayer { get; private set; }

    /// <summary>
    ///     Speichert den Gewinner des Spiels bzw. ob das Spiel bereits vorbei ist.
    ///     Standardwert ist <see cref="Winner.None" />, also kein Gewinner (Spiel läuft noch)
    /// </summary>
    public Winner GameState { get; private set; } = Winner.None;

    /// <summary>
    ///     Array aus Arrays, welches das Spielbrett repräsentiert. Jedes Array in diesem Array enthält
    ///     <see cref="GridEntry" />s und stellt eine Zeile des Brettes dar
    /// </summary>
    public GridEntry[][] Grid { get; }


    /// <summary>
    ///     Diese Methode führt einen Zug für den <see cref="ActivePlayer" /> aus
    /// </summary>
    /// <param name="row">Zeile</param>
    /// <param name="column">Spalte</param>
    /// <exception cref="ArgumentException">
    ///     Wirft, wenn die Koordinaten ungültig sind
    /// </exception>
    public void Move(int row, int column)
    {
        // Wenn das Spiel bereits vorbei ist
        if (GameState != Winner.None)
        {
            throw new InvalidOperationException("Game has ended");
        }

        // Wenn die Koordinaten außerhalb des Spielbrettes liegen
        if (row > 2 || column > 2 || row < 0 || column < 0)
        {
            throw new ArgumentException("Row and Column are out of range.");
        }

        // Wenn an der gewünschten Position bereits etwas steht
        if (Grid[row][column] != GridEntry.None)
        {
            throw new ArgumentException("Cell is occupied.");
        }

        // Feld wird gesetzt
        Grid[row][column] = ActivePlayer;
        // Spieler wird gewechselt
        toggleActivePlayer();
        // Überprüfen, ob das Spiel gewonnen wurde
        checkForWin();
    }

    private void toggleActivePlayer()
    {
        // Wenn der Spieler X ist, dann wechsele zu O, sonst zu X
        ActivePlayer = ActivePlayer == GridEntry.X ? GridEntry.O : GridEntry.X;
    }

    /// <summary>
    ///     Überprüft, ob ein Spieler gewonnen hat und setzt den <see cref="GameState" />
    /// </summary>
    private void checkForWin()
    {
        GameState = CheckForWin(Grid);
    }

    /// <summary>
    ///     Überprüft, ob ein Spieler gewonnen hat und gibt diesen Spieler zurück
    /// </summary>
    /// <param name="board">Das zu analysierende Spielbrett</param>
    /// <returns>Gewinner; <see cref="Winner.None" />, wenn das Spiel noch nicht vorbei ist</returns>
    public static Winner CheckForWin(GridEntry[][] board)
    {
        // Variable deklarieren
        Winner winner;

        // Zeilen überprüfen
        for (int row = 0; row < 3; row++)
        {
            // Wenn die gesamte Zeile gleich ist, dann ist der Gewinner der Wert, der in dieser Zeile steht
            winner = returnIfEqual(board[row][0], board[row][1], board[row][2]);

            // Wenn die Zeile nicht leer war, wurde ein Gewinner gefunden
            if (winner != Winner.None)
            {
                return winner;
            }
        }

        // Spalten überprüfen
        for (int column = 0; column < 3; column++)
        {
            // Gleiches Prinzip
            winner = returnIfEqual(board[0][column], board[1][column], board[2][column]);

            if (winner != Winner.None)
            {
                return winner;
            }
        }

        // Diagonalen überprüfen
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

        // Gleichstand prüfen (Gleichstand ⇔ keine leeren Felder mehr)
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

    /// <summary>
    ///     Überprüft, ob drei <see cref="GridEntry" />s gleich sind und gibt den äquivalenten <see cref="Winner" /> zurück
    /// </summary>
    /// <param name="a">Erster <see cref="GridEntry" /></param>
    /// <param name="b">Zweiter <see cref="GridEntry" /></param>
    /// <param name="c">Dritter <see cref="GridEntry" /></param>
    /// <returns>Den entsprechenden <see cref="Winner" />. <see cref="Winner.X" /> für <see cref="GridEntry.X" /> usw.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static Winner returnIfEqual(GridEntry a, GridEntry b, GridEntry c)
    {
        if (a == b && b == c)
        {
            return a switch
            {
                GridEntry.None => Winner.None, // Wenn die Felder leer sind, kein Gewinner
                GridEntry.X => Winner.X, // Wenn im Feld GridEntry.X steht, Winner.X
                GridEntry.O => Winner.O, // Wenn im Feld GridEntry.O steht, Winner.O
                // Wenn ein anderer Wert kommt (im Moment unmöglich), wird ein Fehler geworfen
                _ => throw new ArgumentOutOfRangeException(nameof(a), a, null)
            };
        }

        // Wenn die Felder nicht gleich sind, gibt es auch keinen Gewinner
        return Winner.None;
    }

    /// <summary>
    ///     Wandelt das Spielbrett in einen <see cref="String" /> um
    /// </summary>
    /// <returns>Spielbrett als <see cref="String" /></returns>
    public string GetAsString()
    {
        // Neuer StringBuilder
        StringBuilder sb = new();

        // Spaltennummern anhängen
        sb.Append("  1 2 3");
        // Zeilenumbruch
        sb.AppendLine();

        for (int row = 0; row < 3; row++)
        {
            // Für jede Zeile die Zeilennummer anhängen
            sb.Append($"{row + 1} ");
            for (int col = 0; col < 3; col++)
            {
                // Für jede Spalte in jeder Zeile den Wert aus dem Spielbrett anhängen

                GridEntry cell = Grid[row][col];
                string value = cell switch
                {
                    GridEntry.X => "X ",
                    GridEntry.O => "O ",
                    _ => "  "
                };
                sb.Append(value);
            }

            // Nach jeder Zeile einen Zeilenumbruch anhängen
            sb.AppendLine();
        }

        // String bauen und zurückgeben
        return sb.ToString();
    }
}