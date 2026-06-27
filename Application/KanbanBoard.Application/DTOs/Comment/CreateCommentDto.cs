namespace KanbanBoard.Application.DTOs.Comment;

public class CreateCommentDto
{
    public Guid TaskId { get; set; }
    public string Content { get; set; } = string.Empty;
}