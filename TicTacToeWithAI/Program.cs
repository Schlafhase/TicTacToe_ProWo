// See https://aka.ms/new-console-template for more information

using TicTacToe;
using TicTacToeWithAI;

Console.WriteLine("Welcome to TicTacToe");

TicTacToeGame game = new();

while (game.GameState == Winner.None)
{
    Console.WriteLine(game.GetAsString());

    if (game.ActivePlayer == GridEntry.X)
    {
        Console.WriteLine("It's the AI's turn!");
        (int row, int col) bestMove = TicTacToeAI.GetBestMove(game.Grid, game.ActivePlayer);

        game.Move(bestMove.row, bestMove.col);

        continue;
    }

    Console.WriteLine("It's your turn!");
    Console.WriteLine("Please enter the coordinates you want to play (seperated by a space):");
    string coordinates = Console.ReadLine()!;

    int x;
    int y;

    try
    {
        x = int.Parse(coordinates.Split(' ')[0]);
        y = int.Parse(coordinates.Split(' ')[1]);
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid coordinates");
        continue;
    }

    if (x is > 3 or < 0 || y is > 3 or < 0)
    {
        Console.WriteLine("Invalid coordinates");
        continue;
    }

    try
    {
        game.Move(y - 1, x - 1);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

Console.WriteLine(game.GetAsString());
Console.WriteLine($"Winner: {game.GameState}");