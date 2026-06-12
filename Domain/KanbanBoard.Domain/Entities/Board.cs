using KanbanBoard.Domain.Common;

public class Board : AuditableEntity
{
    public string Title { get; private set; }

    public Guid WorkspaceId { get; private set; }

    public Workspace Workspace { get; private set; } = null!;

    private Board()
    {
    }

    public Board(
        string title,
        Guid workspaceId)
    {
        Title = title;
        WorkspaceId = workspaceId;
    }

    public ICollection<BoardColumn> Columns
        = new List<BoardColumn>();
}