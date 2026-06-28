using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<Comment?> GetCommentWithUserAsync(Guid commentId)
    {
        return await _dbSet
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByTaskWithUserAsync(Guid taskId)
    {
        return await _dbSet
            .Include(c => c.User)
            .Where(c => c.TaskId == taskId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .Where(c => c.TaskId == taskId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(c => c.Task)
            .ThenInclude(t => t.Column)
            .ThenInclude(c => c.Board)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetCommentCountByTaskAsync(Guid taskId)
    {
        return await _dbSet
            .CountAsync(c => c.TaskId == taskId);
    }

    public async Task DeleteCommentsByTaskAsync(Guid taskId)
    {
        var comments = await _dbSet
            .Where(c => c.TaskId == taskId)
            .ToListAsync();

        if (comments.Any())
        {
            _dbSet.RemoveRange(comments);
        }
    }
}