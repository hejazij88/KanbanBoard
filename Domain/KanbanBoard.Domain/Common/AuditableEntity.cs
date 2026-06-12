namespace KanbanBoard.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}