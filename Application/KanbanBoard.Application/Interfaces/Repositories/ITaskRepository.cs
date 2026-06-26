namespace KanbanBoard.Application.Interfaces.Repositories;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<TaskItem?> GetTaskWithDetailsAsync(Guid taskId);

    Task<IEnumerable<TaskItem>> GetTasksByColumnAsync(Guid columnId);

    Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(Guid boardId);

    Task<IEnumerable<TaskItem>> GetTasksAssignedToUserAsync(Guid userId);

    Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Guid boardId, Domain.Enums.Priority priority);

    Task<IEnumerable<TaskItem>> GetTasksByDueDateRangeAsync(Guid boardId, DateTime from, DateTime to);

    Task<int> GetMaxOrderAsync(Guid columnId);

    Task<int> GetTaskCountByColumnAsync(Guid columnId);

    Task<int> GetTaskCountByBoardAsync(Guid boardId);

    Task<IEnumerable<TaskItem>> SearchTasksAsync(Guid boardId, string searchTerm);

    Task ReorderTasksAsync(Guid columnId, int fromOrder, int toOrder);
}