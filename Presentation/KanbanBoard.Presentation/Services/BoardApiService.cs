using KanbanBoard.Application.DTOs.Board;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class BoardApiService : BaseApiService
{
    public BoardApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<BoardApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<BoardDto>> GetBoardsByWorkspaceAsync(Guid workspaceId, CancellationToken ct = default)
    {
        return await GetAsync<List<BoardDto>>($"api/boards/workspace/{workspaceId}", ct) ?? new();
    }

    public async Task<BoardDto> GetBoardByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<BoardDto>($"api/boards/{id}", ct);
    }

    public async Task<BoardDto> CreateBoardAsync(CreateBoardDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, BoardDto>("api/boards", new { BoardDto = dto }, ct);
    }

    public async Task UpdateBoardAsync(Guid id, UpdateBoardDto dto, CancellationToken ct = default)
    {
        await PutAsync($"api/boards/{id}", new { BoardId = id, BoardDto = dto }, ct);
    }

    public async Task DeleteBoardAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/boards/{id}", ct);
    }
}