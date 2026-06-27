using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities;

public class BoardColumn : AuditableEntity
{
    public string Name { get; private set; }

    public int Order { get; private set; }

    public Guid BoardId { get; private set; }

    public Board Board { get; private set; } = null!;

    private BoardColumn()
    {
    }

    public BoardColumn(
        string name,
        int order,
        Guid boardId)
    {
        Name = name;
        Order = order;
        BoardId = boardId;
    }

    public BoardColumn(
        Guid id,
        string name,
        int order,
        Guid boardId)
    {
        Id = id;
        Name = name;
        Order = order;
        BoardId = boardId;
    }

    public ICollection<TaskItem> Tasks
        = new List<TaskItem>();
}