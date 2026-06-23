namespace KanbanBoard.Application.DTOs.Board;

public class UpdateBoardDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}