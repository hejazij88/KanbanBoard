using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

public class WorkspaceMember 
{


    public Guid Id { get;private set; }
    public DateTime JoinedAt { get;private set; }
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Guid WorkspaceId { get; private set; }

    public Workspace Workspace { get; private set; } = null!;

    public WorkspaceRole Role { get; private set; }

    private WorkspaceMember()
    {
    }

    public WorkspaceMember(Workspace workspace, User user, WorkspaceRole role)
    {
        Id = Guid.NewGuid();
        Workspace = workspace;
        User = user;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    public void SetRole(WorkspaceRole role)
    {
        Role = role;
    }
}