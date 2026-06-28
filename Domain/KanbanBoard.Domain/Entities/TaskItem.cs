using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public Priority Priority { get; private set; }
    public int Order { get;private set; }
    public DateTime? DueDate { get; private set; }

    public Guid ColumnId { get; private set; }

    public BoardColumn Column { get; private set; } = null!;

    public Guid? AssignedUserId { get; private set; }

    public User? AssignedUser { get; private set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    private TaskItem()
    {
    }

    public TaskItem(
        string title,
        string description,
        Priority priority,
        Guid columnId,
        int order)
    {
        Title = title;
        Description = description;
        Priority = priority;
        ColumnId = columnId;
        Order = order;
    }

    public void MoveToColumn(Guid newColumnId, int newOrder)
    {
        ColumnId = newColumnId;
        Order = newOrder;
        DueDate = DateTime.UtcNow;

    }

    public void Reorder(int newOrder)
    {
        if (newOrder < 0)
            throw new ArgumentException("Order must be non-negative.");
        Order = newOrder;
        UpdatedAt = DateTime.UtcNow;
    }
}
