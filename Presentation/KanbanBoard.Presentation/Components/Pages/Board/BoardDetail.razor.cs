using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Presentation.Components.Pages.Column.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Board;

public partial class BoardDetail
{
    [Parameter] public Guid BoardId { get; set; }
    private BoardDto? _board;
    private bool _isLoading = true;
    protected override async System.Threading.Tasks.Task OnInitializedAsync() => await LoadBoard();

    private async System.Threading.Tasks.Task LoadBoard()
    {
        _isLoading = true;
        try
        {
            _board = await BoardApiService.GetBoardByIdAsync(BoardId);
        }
        catch (Exception ex)
        {
            Snackbar.Add("Error loading board: " + ex.Message, Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async System.Threading.Tasks.Task OpenCreateColumnDialog()
    {
        var parameters = new DialogParameters { { "BoardId", BoardId } };
        var dialog = await DialogService.ShowAsync<CreateColumnDialog>("Add Column", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadBoard();
    }
}