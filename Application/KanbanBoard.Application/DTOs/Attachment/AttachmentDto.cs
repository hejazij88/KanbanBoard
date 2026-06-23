namespace KanbanBoard.Application.DTOs.Attachment;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSizeInBytes { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Guid TaskId { get; set; }
    public DateTime UploadedAt { get; set; }
}