using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Entities;

public class Workspace : AuditableEntity
{
    public string Name { get; private set; }
    public Guid OwnerId { get;private set; }
    public User Owner { get;private set; } = null!;
 

    private Workspace()
    {
    }

    public Workspace(string name,Guid ownerId)
    {
        SetName(name);
        OwnerId = ownerId;

    }
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Workspace name cannot be empty.");
        Name = name;
    }

    
    public void AddMember(User user, WorkspaceRole role = WorkspaceRole.Member)
    {
        if (_members.Any(m => m.UserId == user.Id))
            throw new InvalidOperationException("User is already a member.");

        var member = new WorkspaceMember(this, user, role);
        _members.Add(member);
    }

    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member == null)
            throw new InvalidOperationException("User is not a member.");

        if (userId == OwnerId)
            throw new InvalidOperationException("Cannot remove the owner from workspace.");

        _members.Remove(member);
    }

    public Board CreateBoard(string title, string? description = null)
    {
        var board = new Board(title, Id, description);
        _boards.Add(board);
        return board;
    }


    private readonly List<WorkspaceMember> _members = new();
    public IReadOnlyCollection<WorkspaceMember> Members => _members.AsReadOnly();

    private readonly List<Board> _boards = new();
    public IReadOnlyCollection<Board> Boards => _boards.AsReadOnly();
   
}