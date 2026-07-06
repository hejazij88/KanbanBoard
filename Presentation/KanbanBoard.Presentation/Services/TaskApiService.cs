using KanbanBoard.Application.DTOs.Task;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class TaskApiService : BaseApiService
{
    public TaskApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<TaskApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<TaskDto>> GetTasksByColumnAsync(Guid columnId, CancellationToken ct = default)
    {
        return await GetAsync<List<TaskDto>>($"api/tasks/column/{columnId}", ct) ?? new();
    }

    public async Task<List<TaskDto>> GetTasksByBoardAsync(Guid boardId, CancellationToken ct = default)
    {
        return await GetAsync<List<TaskDto>>($"api/tasks/board/{boardId}", ct) ?? new();
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<TaskDto>($"api/tasks/{id}", ct);
    }

    public async Task<TaskDto> CreateTaskAsync(Guid columnId, CreateTaskDto dto, CancellationToken ct = default)
    {
        return await PostAsync<object, TaskDto>(
            $"api/tasks/column/{columnId}",
            new { ColumnId = columnId, TaskDto = dto },
            ct);
    }

    public async Task UpdateTaskAsync(Guid id, UpdateTaskDto dto, CancellationToken ct = default)
    {
        await PutAsync($"api/tasks/{id}", new { TaskId = id, TaskDto = dto }, ct);
    }

    public async Task DeleteTaskAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/tasks/{id}", ct);
    }

    public async Task MoveTaskAsync(MoveTaskDto dto, CancellationToken ct = default)
    {
        await PostAsync("api/tasks/move", new { MoveTaskDto = dto }, ct);
    }

    public async Task<List<TaskDto>> SearchTasksAsync(Guid boardId, string keyword, CancellationToken ct = default)
    {
        return await GetAsync<List<TaskDto>>($"api/tasks/search?boardId={boardId}&keyword={Uri.EscapeDataString(keyword)}", ct) ?? new();
    }
}