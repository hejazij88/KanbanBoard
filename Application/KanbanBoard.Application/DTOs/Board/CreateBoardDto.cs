namespace KanbanBoard.Application.DTOs.Board;

public class CreateBoardDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid WorkspaceId { get; set; }
}