using KanbanBoard.Application.DTOs.Task;

namespace KanbanBoard.Application.DTOs.Column;

public class ColumnDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public Guid BoardId { get; set; }
    public int TaskCount { get; set; }
    public List<TaskDto> Tasks { get; set; } = new();
}