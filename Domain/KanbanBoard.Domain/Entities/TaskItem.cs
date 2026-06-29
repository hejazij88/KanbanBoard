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

    private readonly List<Comment> _comments = new();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private readonly List<Attachment> _attachments = new();
    public IReadOnlyCollection<Attachment> Attachments => _attachments.AsReadOnly();

    private TaskItem()
    {
    }

    public TaskItem(string title, string description, Priority priority, Guid columnId, int order)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        ColumnId = columnId;
        Order = order;
        CreatedAt = DateTime.UtcNow;
    }
    public void UpdateDetails(string title, string description, Priority priority, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToUser(Guid userId)
    {
        AssignedUserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UnassignUser()
    {
        AssignedUserId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
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

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddAttachment(Attachment attachment)
    {
        _attachments.Add(attachment);
        UpdatedAt = DateTime.UtcNow;
    }
}
