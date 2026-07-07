using KanbanBoard.Application.DTOs.Attachment;
using KanbanBoard.Application.DTOs.Comment;
using KanbanBoard.Application.DTOs.Task;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Task.Components;

public partial class TaskDetailDialog
{
    [Parameter] public Guid TaskId { get; set; }
    private TaskDto? _task;
    private CreateCommentDto _commentModel = new();
    private bool _isLoading = true;
    protected override async System.Threading.Tasks.Task OnInitializedAsync() => await LoadTask();

    private async System.Threading.Tasks.Task LoadTask()
    {
        _isLoading = true;
        try
        {
            _task = await TaskApiService.GetTaskByIdAsync(TaskId);
        }
        catch (Exception ex)
        {
            Snackbar.Add("Error loading task: " + ex.Message, Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async System.Threading.Tasks.Task AddComment()
    {
        try
        {
            await CommentApiService.AddCommentAsync(TaskId, _commentModel);
            await LoadTask();
            _commentModel = new();
            Snackbar.Add("Comment added!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async System.Threading.Tasks.Task UploadAttachment(InputFileChangeEventArgs e)
    {
        try
        {
            var file = e.File;
            var model = new CreateAttachmentDto { File = file };
            await AttachmentApiService.UploadAttachmentAsync(TaskId, model);
            await LoadTask();
            Snackbar.Add("File uploaded successfully!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async System.Threading.Tasks.Task DeleteAttachment(Guid attachmentId)
    {
        try
        {
            await AttachmentApiService.DeleteAttachmentAsync(attachmentId);
            await LoadTask();
            Snackbar.Add("Attachment deleted!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async System.Threading.Tasks.Task OpenEditTaskDialog()
    {
        var parameters = new DialogParameters { { "Task", _task } };
        var dialog = await DialogService.ShowAsync<EditTaskDialog>("Edit Task", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadTask();
    }
}