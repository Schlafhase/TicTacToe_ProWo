using Microsoft.AspNetCore.Components;
using TicTacToe;

namespace TicTacToeMultiplayer.Components.Pages;

public partial class Home : ComponentBase, IDisposable
{
    [Parameter] public string? PlayerAsString { get; set; }

    private readonly GameManager _gameManager;
    private GridEntry _player => PlayerAsString == "O" ? GridEntry.O : GridEntry.X;
    private string _info = string.Empty;

    public Home(GameManager gameManager)
    {
        _gameManager = gameManager;
        _gameManager.OnMove += invokeStateChange;
        _gameManager.CountdownChanged += invokeStateChange;
        _gameManager.OnReset += invokeStateChange;
    }

    private async void invokeStateChange()
    {
        _info = "";
        await InvokeAsync(StateHasChanged);
    }

    private void move(int row, int column)
    {
        if (_player != _gameManager.Game.ActivePlayer)
        {
            _info = "It's not your turn!";
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

    public void Dispose()
    {
        _gameManager.OnMove -= invokeStateChange;
        _gameManager.CountdownChanged -= invokeStateChange;
        _gameManager.OnReset -= invokeStateChange;
    }
}