using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.DTOs.Workspace;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Workspace;

public partial class WorkspaceDetail
{
    [Parameter] public Guid WorkspaceId { get; set; }
    private WorkspaceDto? _workspace;
    private List<BoardDto> _boards = new();
    private bool _isLoading = true;
    protected override async Task OnInitializedAsync() => await LoadData();

    private async Task LoadData()
    {
        _isLoading = true;
        try
        {
            _workspace = await WorkspaceApiService.GetWorkspaceByIdAsync(WorkspaceId);
            _boards = await BoardApiService.GetBoardsByWorkspaceAsync(WorkspaceId);
        }
        catch (Exception ex)
        {
            Snackbar.Add("Error loading data: " + ex.Message, Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task OpenCreateBoardDialog()
    {
        var parameters = new DialogParameters { { "WorkspaceId", WorkspaceId } };
        var dialog = await DialogService.ShowAsync<CreateBoardDialog>("Create Board", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadData();
    }
}