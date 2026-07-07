using KanbanBoard.Application.DTOs.Task;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Task.Components;

public partial class EditTaskDialog
{
    [CascadingParameter] private DialogParameters? Parameters { get; set; }
    private UpdateTaskDto _model = new();
    private Guid _taskId;

    protected override void OnInitialized()
    {
        if (Parameters != null && Parameters.TryGetValue("Task", out object? taskObj))
        {
            var task = (TaskDto)taskObj!;
            _taskId = task.Id;
            _model.Title = task.Title;
            _model.Description = task.Description;
            _model.Priority = task.Priority;
            _model.DueDate = task.DueDate;
            _model.AssignedUserId = task.AssignedUserId;
        }
    }

    private async System.Threading.Tasks.Task UpdateTask()
    {
        try
        {
            await TaskApiService.UpdateTaskAsync(_taskId, _model);
            Dialog.CloseAsync(DialogResult.Ok(true));
            Snackbar.Add("Task updated!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}