using KanbanBoard.Application.DTOs.Column;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Column.Components;

public partial class ColumnCard
{
    [Parameter] public ColumnDto Column { get; set; } = null!;
    [Parameter] public EventCallback OnTaskMoved { get; set; }

    private async Task OpenCreateTaskDialog()
    {
        var parameters = new DialogParameters { { "ColumnId", Column.Id } };
        var dialog = await DialogService.ShowAsync<CreateTaskDialog>("Add Task", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
            await OnTaskMoved.InvokeAsync();
    }
}