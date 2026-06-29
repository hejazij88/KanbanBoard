namespace KanbanBoard.Domain.Common;

public abstract class AuditableEntity
{

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    protected AuditableEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

}