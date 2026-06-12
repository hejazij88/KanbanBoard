using KanbanBoard.Domain.Common;

public class Workspace : AuditableEntity
{
    public string Name { get; private set; }

    private Workspace()
    {
    }

    public Workspace(string name)
    {
        Name = name;
    }

    public ICollection<WorkspaceMember> Members
        = new List<WorkspaceMember>();

    public ICollection<Board> Boards
        = new List<Board>();
}