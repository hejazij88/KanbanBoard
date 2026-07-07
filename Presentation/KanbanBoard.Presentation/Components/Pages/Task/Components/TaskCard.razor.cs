using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Task.Components;

public partial class TaskCard
{
    [Parameter] public TaskDto Task { get; set; } = null!;
    [Parameter] public EventCallback OnTaskUpdated { get; set; }

    private Color GetPriorityColor(Priority priority) => priority switch
    {
        Priority.Low => Color.Success,
        Priority.Medium => Color.Info,
        Priority.High => Color.Warning,
        Priority.Critical => Color.Error,
        _ => Color.Default
    };

    private Color GetDueDateColor(DateTime dueDate)
    {
        var days = (dueDate - DateTime.Today).Days;
        return days < 0 ? Color.Error : days < 3 ? Color.Warning : Color.Success;
    }

    private async System.Threading.Tasks.Task OpenTaskDetail()
    {
        var parameters = new DialogParameters { { "TaskId", Task.Id } };
        var dialog = await DialogService.ShowAsync<TaskDetailDialog>("Task Details", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
            await OnTaskUpdated.InvokeAsync();
    }
}