using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<Attachment?> GetAttachmentWithTaskAsync(Guid attachmentId)
    {
        return await _dbSet
            .Include(a => a.Task)
            .ThenInclude(t => t.Column)
            .FirstOrDefaultAsync(a => a.Id == attachmentId);
    }

    public async Task<IEnumerable<Attachment>> GetAttachmentsByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .Where(a => a.TaskId == taskId)
            .OrderByDescending(a => a.UploadedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attachment>> GetAttachmentsByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(a => a.Task)
            .Where(a => a.Task.AssignedUserId == userId)
            .OrderByDescending(a => a.UploadedAt)
            .ToListAsync();
    }

    public async Task<int> GetAttachmentCountByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .CountAsync(a => a.TaskId == taskId);
    }

    public async Task<long> GetTotalFileSizeByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .Where(a => a.TaskId == taskId)
            .SumAsync(a => (long?)a.FileSize) ?? 0;
    }

    public async Task DeleteAttachmentsByTaskAsync(Guid taskId)
    {
        var attachments = await _dbSet
            .Where(a => a.TaskId == taskId)
            .ToListAsync();

        if (attachments.Any())
        {
            _dbSet.RemoveRange(attachments);
        }
    }

    public async Task<bool> IsFileNameExistsInTaskAsync(Guid taskId, string fileName)
    {
        return await _dbSet
            .AnyAsync(a => a.TaskId == taskId && a.FileName == fileName);
    }
}