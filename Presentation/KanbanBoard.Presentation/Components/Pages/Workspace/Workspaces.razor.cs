using KanbanBoard.Application.DTOs.Workspace;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Workspace;

public partial class Workspaces
{
    private List<WorkspaceDto> _workspaces = new();
    private bool _isLoading = true;
    protected override async Task OnInitializedAsync() => await LoadWorkspaces();

    private async Task LoadWorkspaces()
    {
        _isLoading = true;
        try
        {
            _workspaces = await WorkspaceApiService.GetWorkspacesAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add("Error loading workspaces: " + ex.Message, Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task OpenCreateWorkspaceDialog()
    {
        var dialog = await DialogService.ShowAsync<CreateWorkspaceDialog>("Create Workspace");
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await LoadWorkspaces();
            Snackbar.Add("Workspace created successfully!", Severity.Success);
        }
    }
}