using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class WorkspaceRepository : Repository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<Workspace?> GetWorkspaceWithMembersAsync(Guid workspaceId)
    {
        return await _dbSet
            .Include(w => w.Owner)
            .Include(w => w.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(w => w.Id == workspaceId);
    }

    public async Task<Workspace?> GetWorkspaceWithBoardsAsync(Guid workspaceId)
    {
        return await _dbSet
            .Include(w => w.Boards)
            .ThenInclude(b => b.Columns)
            .FirstOrDefaultAsync(w => w.Id == workspaceId);
    }

    public async Task<IEnumerable<Workspace>> GetWorkspacesByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(w => w.Members)
            .Include(w => w.Owner)
            .Where(w => w.OwnerId == userId || w.Members.Any(m => m.UserId == userId))
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Workspace>> GetWorkspacesOwnedByUserAsync(Guid userId)
    {
        return await _dbSet
            .Where(w => w.OwnerId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsUserMemberAsync(Guid workspaceId, Guid userId)
    {
        return await _dbSet
            .AnyAsync(w => w.Id == workspaceId &&
                           (w.OwnerId == userId || w.Members.Any(m => m.UserId == userId)));
    }

    public async Task<string?> GetUserRoleInWorkspaceAsync(Guid workspaceId, Guid userId)
    {
        var workspace = await _dbSet
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w => w.Id == workspaceId);

        if (workspace == null)
            return null;

        if (workspace.OwnerId == userId)
            return "Admin";

        return workspace.Members.FirstOrDefault(m => m.UserId == userId)?.Role.ToString();
    }

    public async Task<int> GetMemberCountAsync(Guid workspaceId)
    {
        return await _context.WorkspaceMembers
            .CountAsync(wm => wm.WorkspaceId == workspaceId);
    }

    public async Task RemoveMemberAsync(Guid workspaceId, Guid userId)
    {
        var member = await _context.WorkspaceMembers
            .FirstOrDefaultAsync(wm => wm.WorkspaceId == workspaceId && wm.UserId == userId);

        if (member != null)
        {
            _context.WorkspaceMembers.Remove(member);
        }
    }
}