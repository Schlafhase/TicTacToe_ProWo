using Microsoft.AspNetCore.Components;
using TicTacToe;

namespace TicTacToeBlazor.Components.Pages;

public partial class Home : ComponentBase
{
    private TicTacToeGame _game = new TicTacToeGame();
    private string _info = string.Empty;

    private void move(int row, int column)
    {
        _info = "";
        try
        {
            _game.Move(row, column);
        }
        catch (Exception ex)
        {
            _info = ex.Message;
            StateHasChanged();
        }
    }
}