using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.DTOs.Task;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
    public Guid? AssignedUserId { get; set; } 
}