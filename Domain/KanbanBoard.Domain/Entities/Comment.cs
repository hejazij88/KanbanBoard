using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities;

public class Comment: AuditableEntity
{
    public string Content { get;private set; } = string.Empty;
    public Guid TaskId { get; private set; }
    public TaskItem Task { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get;private set; } = null!;
    public DateTime CreatedAt { get;private set; }

    public Comment(string content,Guid userId,Guid taskId)
    {
        Id = Guid.NewGuid();
        SetContent(content);
        Content = content;
        UserId=userId;
        TaskId=taskId;
        CreatedAt= DateTime.UtcNow;
    }

    public void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentNullException("Cpmment Content can't be empty");

        Content= content;
        UpdatedAt=DateTime.UtcNow;
    }
}