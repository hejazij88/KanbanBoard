using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);


    public async Task<User?> GetUserWithDetailsAsync(Guid userId) => await _dbSet.Include(u => u.WorkspaceMembers)
        .ThenInclude(w => w.Workspace).Include(u => u.AssignedTasks).Include(u => u.Comments)
        .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken) =>
        await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

    public async Task UpdateRefreshTokenAsync(Guid userId, string? refreshToken, DateTime? expiryTime)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            //user.SetRefreshToken(refreshToken, expiryTime);
            Update(user);
        }
    }

    public async Task<IEnumerable<User>> GetUsersByWorkspaceRoleAsync(Guid workspaceId, string role)
    {

        return _dbSet.Where(u => u.WorkspaceMembers.Any(wm => wm.WorkspaceId == workspaceId && wm.Role.ToString() == role));
    }

    public async Task<IEnumerable<User>> GetUsersNotInWorkspaceAsync(Guid workspaceId)
    {
        return await _dbSet
            .Where(u => !u.WorkspaceMembers.Any(wm => wm.WorkspaceId == workspaceId))
            .ToListAsync();
    }
}