using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Enums;

public class TaskItem : AuditableEntity
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public Priority Priority { get; private set; }

    public DateTime? DueDate { get; private set; }

    public Guid ColumnId { get; private set; }

    public BoardColumn Column { get; private set; } = null!;

    public Guid? AssignedUserId { get; private set; }

    public User? AssignedUser { get; private set; }

    private TaskItem()
    {
    }

    public TaskItem(
        string title,
        string description,
        Priority priority,
        Guid columnId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        ColumnId = columnId;
    }
}