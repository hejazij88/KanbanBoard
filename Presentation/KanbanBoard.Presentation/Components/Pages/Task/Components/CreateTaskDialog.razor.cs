using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Task.Components;

public partial class CreateTaskDialog
{
    [CascadingParameter] private DialogParameters? Parameters { get; set; }
    private CreateTaskDto _model = new() { Priority = Priority.Medium };
    private Guid _columnId;

    protected override void OnInitialized()
    {
        if (Parameters != null && Parameters.TryGetValue("ColumnId", out object? columnId))
            _columnId = (Guid)columnId!;
    }

    private async System.Threading.Tasks.Task CreateTask()
    {
        try
        {
            var result = await TaskApiService.CreateTaskAsync(_columnId, _model);
            Dialog.CloseAsync(DialogResult.Ok(result));
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}