using KanbanBoard.Application.DTOs.Workspace;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class WorkspaceApiService : BaseApiService
{
    public WorkspaceApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<WorkspaceApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<WorkspaceDto>> GetWorkspacesAsync(CancellationToken ct = default)
    {
        return await GetAsync<List<WorkspaceDto>>("api/workspaces", ct) ?? new();
    }

    public async Task<WorkspaceDto> GetWorkspaceByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<WorkspaceDto>($"api/workspaces/{id}", ct);
    }

    public async Task<WorkspaceDto> CreateWorkspaceAsync(CreateWorkspaceDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, WorkspaceDto>("api/workspaces", new { WorkspaceDto = dto }, ct);
    }

    public async Task UpdateWorkspaceAsync(Guid id, UpdateWorkspaceDto dto, CancellationToken ct = default)
    {
        await PutAsync($"api/workspaces/{id}", new { WorkspaceId = id, WorkspaceDto = dto }, ct);
    }

    public async Task DeleteWorkspaceAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/workspaces/{id}", ct);
    }

    public async Task<WorkspaceMemberDto> AddMemberAsync(Guid workspaceId, AddMemberDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, WorkspaceMemberDto>(
            $"api/workspaces/{workspaceId}/members",
            new { WorkspaceId = workspaceId, AddMemberDto = dto },
            ct);
    }

    public async Task RemoveMemberAsync(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        await DeleteAsync($"api/workspaces/{workspaceId}/members/{userId}", ct);
    }
}