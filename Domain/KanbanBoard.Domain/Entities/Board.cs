using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities;

public class Board : AuditableEntity
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Guid WorkspaceId { get; private set; }

    public Workspace Workspace { get; private set; } = null!;

    private Board()
    {
    }

    public Board(string title, Guid workspaceId, string? description = null)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        WorkspaceId = workspaceId;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Board title cannot be empty.");
        Title = title;
    }

    public BoardColumn AddColumn(string title, int order)
    {
        var column = new BoardColumn(title, Id, order);
        _columns.Add(column);
        return column;
    }

    public void RemoveColumn(Guid columnId)
    {
        var column = _columns.FirstOrDefault(c => c.Id == columnId);
        if (column == null)
            throw new InvalidOperationException("Column not found.");

        if (column.Tasks.Any())
            throw new InvalidOperationException("Cannot delete a column with tasks.");

        _columns.Remove(column);
    }

    public void CreateDefaultColumns()
    {
        AddColumn("Todo", 1);
        AddColumn("In Progress", 2);
        AddColumn("Done", 3);
    }

    private readonly List<BoardColumn> _columns = new();
    public IReadOnlyCollection<BoardColumn> Columns => _columns;
}