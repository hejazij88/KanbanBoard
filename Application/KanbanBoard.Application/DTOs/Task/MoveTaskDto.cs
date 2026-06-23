namespace KanbanBoard.Application.DTOs.Task;

public class MoveTaskDto
{
    public Guid TaskId { get; set; }
    public Guid TargetColumnId { get; set; }
    public int? NewOrder { get; set; } 
}