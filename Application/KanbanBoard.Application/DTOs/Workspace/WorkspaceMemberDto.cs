namespace KanbanBoard.Application.DTOs.Workspace;

public class WorkspaceMemberDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; 
    public DateTime JoinedAt { get; set; }
}