using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IWorkspaceRepository : IRepository<Workspace>
{
    Task RemoveMemberAsync(Guid workspaceId, Guid userId);

    Task<int> GetMemberCountAsync(Guid workspaceId);

    Task<string?> GetUserRoleInWorkspaceAsync(Guid workspaceId, Guid userId);

    Task<bool> IsUserMemberAsync(Guid workspaceId, Guid userId);

    Task<IEnumerable<Workspace>> GetWorkspacesOwnedByUserAsync(Guid userId);

    Task<IEnumerable<Workspace>> GetWorkspacesByUserAsync(Guid userId);

    Task<Workspace?> GetWorkspaceWithBoardsAsync(Guid workspaceId);

    Task<Workspace?> GetWorkspaceWithMembersAsync(Guid workspaceId);
}