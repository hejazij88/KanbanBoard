namespace KanbanBoard.Application.DTOs.Column;

public class UpdateColumnDto
{
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
}