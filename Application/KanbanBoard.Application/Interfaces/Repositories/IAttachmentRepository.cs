namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IAttachmentRepository : IRepository<Attachment>
{
    Task<Attachment?> GetAttachmentWithTaskAsync(Guid attachmentId);

    Task<IEnumerable<Attachment>> GetAttachmentsByTaskAsync(Guid taskId);

    Task<IEnumerable<Attachment>> GetAttachmentsByUserAsync(Guid userId);

    Task<int> GetAttachmentCountByTaskAsync(Guid taskId);

    Task<long> GetTotalFileSizeByTaskAsync(Guid taskId);

    Task DeleteAttachmentsByTaskAsync(Guid taskId);

    Task<bool> IsFileNameExistsInTaskAsync(Guid taskId, string fileName);
}