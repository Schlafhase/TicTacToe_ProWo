using System.Text;
using TicTacToe;

namespace TicTacToeWithAI;

public static class TicTacToeAI
{
    /// <summary>
    ///     Gibt einen tuple zurück, der den besten Zug enthält
    /// </summary>
    public static (int row, int col) GetBestMove(GridEntry[][] board, GridEntry aiSymbol)
    {
        // Beste Punktzahl am Anfang auf den kleinstmöglichen Wert setzen
        int bestScore = int.MinValue;
        // Bester Zug ist am Anfang undefiniert/ungültig
        (int row, int col) bestMove = (-1, -1);

        // Jeden möglichen Zug ausprobieren und die Punktzahl berechnen
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                // Wenn die Zelle besetzt ist, ist der Zug ungültig
                if (board[row][col] != GridEntry.None)
                {
                    continue;
                }

                // Symbol setzen und schauen, wie sich die Punktzahl ändert
                board[row][col] = aiSymbol;
                // Punktzahl wird mit dem miniMax-Algorithmus berechnet
                // Negative Punktzahlen bedeuten, dass der Mensch einen Vorteil hat; Positive Punktzahlen bedeuten, dass die AI einen Vorteil hat
                int score = miniMax(board, aiSymbol);
                // Spielbrett wieder zurücksetzen (Zug wurde nur temporär ausgeführt, um ihn zu testen)
                board[row][col] = GridEntry.None;

                // Wenn der Zug besser war als der vorherige beste Zug
                if (score > bestScore)
                {
                    // Beste Punktzahl aktualisieren
                    bestScore = score;
                    // Dieser Zug ist der neue beste Zug
                    bestMove = (row, col);
                    Console.WriteLine($"AI: Found new best move: {bestMove.row}, {bestMove.col} with score {score}");
                }
            }
        }

        return bestMove;
    }

    public static string GetAsString(GridEntry[][] board)
    {
        StringBuilder sb = new();

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

    /// <summary>
    ///     Berechnet die Punktzahl für einen Zustand des Bretts
    /// </summary>
    /// <param name="board">Der Zustand des Spielbretts, für den die Punktzahl berechnet werden soll</param>
    /// <param name="aiSymbol">Das Symbol (X oder O), das von der AI gespielt wird</param>
    /// <param name="depth">Die Tiefe der rekursiven Aufrufe. Sollte nur beim rekursiven Aufruf der Methode gesetzt werden.</param>
    /// <param name="maximising">Gibt and, ob hohe oder niedrige Punktzahlen besser sind. (Wenn die AI spielt, ist maximising true)</param>
    /// <returns>
    ///     Die Punktzahl, die durch den miniMax-Algorithmus berechnet wurde
    /// </returns>
    private static int miniMax(GridEntry[][] board, GridEntry aiSymbol, int depth = 0,
        bool maximising = false)
    {
        // Überprüfen, ob es einen Gewinner gibt
        Winner winner = TicTacToeGame.CheckForWin(board);

        // Wenn das Spiel vorbei ist (Rekursionsanker)
        if (winner != Winner.None)
        {
            // 0 Punkte bei Gleichstand (kein Spieler hat einen Vorteil)
            if (winner == Winner.Draw)
            {
                return 0;
            }
            // Wenn die AI gewonnen hat
            if (winner.ToString() == aiSymbol.ToString())
            {
                // Zehn Punkte für den Sieg; Es werden Punkte abgezogen, für jeden Zug der gebraucht wird um zu gewinnen.
                // Dadurch werden Positionen, in denen die AI schnell gewinnen kann, bevorzugt
                return 10 - depth;
            }

            // Wenn der Mensch gewinnt, wird die Berechnung umgedreht (für den Menschen bedeuten niedrige Punktzahlen bessere Positionen)
            return depth - 10;
        }

        
        if (maximising)
        {
            // Spieler am Zug ist die AI -> will hohe Punktzahl
            int bestScore = int.MinValue;

            // Jeden Zug ausprobieren
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Wenn die Zelle besetzt ist, ist der Zug ungültig
                    if (board[row][col] != GridEntry.None)
                    {
                        continue;
                    }

                    // aiSymbol setzen, um den Zug auszuprobieren
                    board[row][col] = aiSymbol;
                    // Punktzahl mit dem miniMax-Algorithmus berechnen (rekursiver Aufruf)
                    // Tiefe wird um 1 erhöht
                    // maximising ist false, weil der nächste Zug vom Menschen ausgeführt wird
                    // Wir berechnen die beste Punktzahl, die die AI bekommen kann, wenn der Mensch selbst auch den bestmöglichen Zug macht
                    int score = miniMax(board, aiSymbol, depth + 1, false);
                    // Brett zurücksetzen
                    board[row][col] = GridEntry.None;
                    bestScore = Math.Max(bestScore, score); // Höhere Punktzahl = besser
                }
            }

            return bestScore;
        }
        else
        {
            // Spieler ist Mensch -> will niedrige Punktzahl
            int bestScore = int.MaxValue;

            // Jeden Zug ausprobieren
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Ungültiger Zug
                    if (board[row][col] != GridEntry.None)
                    {
                        continue;
                    }

                    // Zelle auf das Symbol des Menschen setzen, um den Zug auszuprobieren
                    board[row][col] = otherSymbol(aiSymbol);
                    // Punktzahl für den neuen Zustand berechnen
                    int score = miniMax(board, aiSymbol, depth + 1, true);
                    // Brett zurücksetzen
                    board[row][col] = GridEntry.None;
                    bestScore = Math.Min(bestScore, score); // Niedrige Punktzahl = besser
                }
            }

            // Beste Punktzahl zurückgeben
            return bestScore;
        }
    }

    /// <summary>
    ///     Helfer-Methode, die einen <see cref="GridEntry"> nimmt, und den Gegner zurückgibt.
    /// </summary>
    private static GridEntry otherSymbol(GridEntry symbol)
    {
        return symbol switch
        {
            GridEntry.O => GridEntry.X, // Gegner von O ist X
            GridEntry.X => GridEntry.O, // Gegner von X ist O
            _ => throw new ArgumentException("symbol must be X or O") // Alle anderen Eingaben machen keinen Sinn
        };
    }
}
