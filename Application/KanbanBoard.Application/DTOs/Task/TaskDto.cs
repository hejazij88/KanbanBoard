namespace KanbanBoard.Application.DTOs.Task;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int Order { get; set; }
    public Guid ColumnId { get; set; }
    public string ColumnTitle { get; set; } = string.Empty;

    public Guid? AssignedUserId { get; set; }
    public string? AssignedUsername { get; set; }
    public string? AssignedUserEmail { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<CommentDto> Comments { get; set; } = new();
    public List<AttachmentDto> Attachments { get; set; } = new();
}