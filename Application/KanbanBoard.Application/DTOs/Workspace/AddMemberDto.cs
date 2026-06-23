namespace KanbanBoard.Application.DTOs.Workspace;

public class AddMemberDto
{
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Member"; // Admin, Member
}