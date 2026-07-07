using KanbanBoard.Application.DTOs.Column;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Column.Components;

public partial class CreateColumnDialog
{
    [CascadingParameter] private DialogParameters? Parameters { get; set; }
    private CreateColumnDto _model = new();

    protected override void OnInitialized()
    {
        if (Parameters != null && Parameters.TryGetValue("BoardId", out object? boardId))
        {
            // BoardId در خود Command استفاده می‌شود و در DTO نیست
        }
    }

    private async Task CreateColumn()
    {
        try
        {
            var boardId = Parameters!.Get<Guid>("BoardId");
            var result = await ColumnApiService.CreateColumnAsync(boardId, _model);
            Dialog.CloseAsync(DialogResult.Ok(result));
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}