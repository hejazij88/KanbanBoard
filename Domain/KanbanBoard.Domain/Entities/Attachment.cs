    using KanbanBoard.Domain.Common;

    namespace KanbanBoard.Domain.Entities;

public class Attachment:AuditableEntity
{
    public string FileName { get;private set; } = string.Empty;
    public string FilePath { get;private set; } = string.Empty;
    public long FileSize { get; private set; }
    public string ContentType { get; private set; }
    public Guid TaskId { get; private set; }
    public TaskItem Task { get; private set; } = null!;
    public DateTime UploadedAt { get;private set; }


    public Attachment()
    {
        
    }

    public Attachment(string fileName, string filePath, long fileSize, string contentType, Guid taskId)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        FilePath = filePath;
        fileSize = fileSize;
        ContentType = contentType;
        TaskId = taskId;
        UploadedAt = DateTime.UtcNow;
    }
}