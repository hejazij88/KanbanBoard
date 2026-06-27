namespace KanbanBoard.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public string Content { get;private set; } = string.Empty;
    public Guid TaskId { get; private set; }
    public TaskItem Task { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get;private set; } = null!;
    public DateTime CreatedAt { get;private set; }

    public Comment(string content,Guid userId,Guid taskId)
    {
        Content = content;
        UserId=userId;
        TaskId=taskId;
        CreatedAt= DateTime.UtcNow;
    }
}