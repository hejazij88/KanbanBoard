using KanbanBoard.Application.DTOs.Comment;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class CommentApiService : BaseApiService
{
    public CommentApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<CommentApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<CommentDto>> GetCommentsByTaskAsync(Guid taskId, CancellationToken ct = default)
    {
        return await GetAsync<List<CommentDto>>($"api/comments/task/{taskId}", ct) ?? new();
    }

    public async Task<CommentDto> AddCommentAsync(Guid taskId, CreateCommentDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, CommentDto>(
            $"api/comments/task/{taskId}",
            new { TaskId = taskId, CommentDto = dto },
            ct);
    }

    public async Task DeleteCommentAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/comments/{id}", ct);
    }
}