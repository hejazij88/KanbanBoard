using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.DTOs.Workspace;

public class AddMemberDto
{
    public string Email { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; } 
}