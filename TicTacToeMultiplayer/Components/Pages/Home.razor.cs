using Microsoft.AspNetCore.Components;
using TicTacToe;

namespace TicTacToeMultiplayer.Components.Pages;

public partial class Home : ComponentBase, IDisposable
{
    private readonly GameManager _gameManager;
    private readonly Timer _timer;
    private string _info = string.Empty;


    public Home(GameManager gameManager)
    {
        _gameManager = gameManager;

        _timer = new Timer(_ => InvokeAsync(StateHasChanged), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
    }

    [Parameter] public string? PlayerAsString { get; set; }

    private GridEntry _player => PlayerAsString == "O" ? GridEntry.O : GridEntry.X;


    public void Dispose()
    {
        _timer.Dispose();
    }

    private void move(int row, int column)
    {
        if (_player != _gameManager.Game.ActivePlayer)
        {
            StateHasChanged();
            return;
        }

        _info = "";
        try
        {
            _gameManager.Game.Move(row, column);
        }
        catch (Exception ex)
        {
            _info = ex.Message;
            StateHasChanged();
        }
    }
}