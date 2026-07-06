using KanbanBoard.Application.DTOs.Column;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class ColumnApiService : BaseApiService
{
    public ColumnApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<ColumnApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<ColumnDto>> GetColumnsByBoardAsync(Guid boardId, CancellationToken ct = default)
    {
        return await GetAsync<List<ColumnDto>>($"api/columns/board/{boardId}", ct) ?? new();
    }

    public async Task<ColumnDto> GetColumnByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<ColumnDto>($"api/columns/{id}", ct);
    }

    public async Task<ColumnDto> CreateColumnAsync(Guid boardId, CreateColumnDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, ColumnDto>(
            $"api/columns/board/{boardId}",
            new { BoardId = boardId, ColumnDto = dto },
            ct);
    }

    public async Task UpdateColumnAsync(Guid id, UpdateColumnDto dto, CancellationToken ct = default)
    {
        await PutAsync($"api/columns/{id}", new { ColumnId = id, ColumnDto = dto }, ct);
    }

    public async Task DeleteColumnAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/columns/{id}", ct);
    }

    public async Task ReorderColumnsAsync(Guid boardId, int oldOrder, int newOrder, CancellationToken ct = default)
    {
        await PostAsync(
            "api/columns/reorder",
            new { BoardId = boardId, OldOrder = oldOrder, NewOrder = newOrder },
            ct);
    }
}