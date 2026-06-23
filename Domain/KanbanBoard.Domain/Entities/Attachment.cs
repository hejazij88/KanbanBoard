public class Attachment
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}