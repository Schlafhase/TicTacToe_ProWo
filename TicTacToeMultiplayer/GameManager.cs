using TicTacToe;

namespace TicTacToeMultiplayer;

public sealed class GameManager
{
    public required TicTacToeGame Game;

    public GameManager()
    {
        NewGame();
    }

    public void NewGame()
    {
        Game = new TicTacToeGame();
    }
}