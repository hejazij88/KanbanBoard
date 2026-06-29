namespace KanbanBoard.Application.DTOs.Workspace;

public class WorkspaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public string OwnerUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int MemberCount { get; set; }
    public int BoardCount { get; set; }
    public List<WorkspaceMemberDto> Members { get; set; } = new();
}