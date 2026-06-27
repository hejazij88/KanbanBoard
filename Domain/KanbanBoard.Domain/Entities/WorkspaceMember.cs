using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

public class WorkspaceMember : BaseEntity
{
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Guid WorkspaceId { get; private set; }

    public Workspace Workspace { get; private set; } = null!;

    public WorkspaceRole Role { get; private set; }

    private WorkspaceMember()
    {
    }

    public WorkspaceMember(
        Guid userId,
        Guid workspaceId,
        WorkspaceRole role)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        Role = role;
    }
}