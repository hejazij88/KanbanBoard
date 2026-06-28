using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class BoardRepository : Repository<Board>, IBoardRepository
{
    public BoardRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<Board?> GetBoardWithColumnsAndTasksAsync(Guid boardId)
    {
        return await _dbSet
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.AssignedUser)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.Comments)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.Attachments)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public async Task<Board?> GetBoardWithColumnsAsync(Guid boardId)
    {
        return await _dbSet
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkspaceAsync(Guid workspaceId)
    {
        return await _dbSet
            .Include(b => b.Columns)
            .Where(b => b.WorkspaceId == workspaceId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Board>> GetBoardsByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(b => b.Workspace)
            .ThenInclude(w => w.Members)
            .Where(b => b.Workspace.OwnerId == userId ||
                        b.Workspace.Members.Any(m => m.UserId == userId))
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetBoardCountAsync(Guid workspaceId)
    {
        return await _dbSet
            .CountAsync(b => b.WorkspaceId == workspaceId);
    }

    public async Task<bool> IsTitleExistsInWorkspaceAsync(Guid workspaceId, string title, Guid? excludeBoardId = null)
    {
        var query = _dbSet
            .Where(b => b.WorkspaceId == workspaceId && b.Title == title);

        if (excludeBoardId.HasValue)
            query = query.Where(b => b.Id != excludeBoardId.Value);

        return await query.AnyAsync();
    }
}