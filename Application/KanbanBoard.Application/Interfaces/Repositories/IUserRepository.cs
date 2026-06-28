using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);

    Task<User?> GetUserWithDetailsAsync(Guid userId);

    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    Task UpdateRefreshTokenAsync(Guid userId, string? refreshToken, DateTime? expiryTime);

    Task<IEnumerable<User>> GetUsersByWorkspaceRoleAsync(Guid workspaceId, string role);

    Task<IEnumerable<User>> GetUsersNotInWorkspaceAsync(Guid workspaceId);

}