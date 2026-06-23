namespace KanbanBoard.Application.DTOs.Column;

public class CreateColumnDto
{
    public string Title { get; set; } = string.Empty;
    public int? Order { get; set; }
}