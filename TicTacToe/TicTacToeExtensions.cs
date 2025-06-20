using System.Text;

namespace TicTacToe;

public static class TicTacToeExtensions
{
    public static string GetAsString(this TicTacToeGame game)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("  1 2 3");
        sb.AppendLine();

        for (int row = 0; row < 3; row++)
        {
            sb.Append($"{row + 1} ");
            for (int col = 0; col < 3; col++)
            {
                GridEntry cell = game[row, col];
                string value = cell.GetAsString() + " ";
                sb.Append(value);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static string GetAsString(this GridEntry entry)
    {
        return entry switch
        {
            GridEntry.X => "X",
            GridEntry.O => "O",
            _ => " "
        };
    }
}