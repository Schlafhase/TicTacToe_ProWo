using TicTacToe;

namespace TicTacToeMultiplayer;

public sealed class GameManager : IDisposable
{
    public required TicTacToeGame Game;
    public int Countdown;
    public event Action? CountdownChanged;
    public event Action? OnMove;
    public event Action? OnReset;

    public GameManager()
    {
        newGame();
    }
    
    private void newGame()
    {
        Game = new TicTacToeGame();
        Game.GameOver += onGameOver;
        Game.OnMove += () => OnMove?.Invoke();
        OnReset?.Invoke();
    }

    private void onGameOver()
    {
        new Thread(() =>
        {
            for (int i = 5; i > 0; i--)
            {
                Countdown = i;
                CountdownChanged?.Invoke();
                Thread.Sleep(1000);
            }
            newGame();
        }).Start();
    }


    public void Dispose()
    {
        Game.GameOver -= onGameOver;
    }
}