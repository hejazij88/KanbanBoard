using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    public TaskRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<TaskItem?> GetTaskWithDetailsAsync(Guid taskId)
    {
        return await _dbSet
            .Include(t => t.Column)
            .Include(t => t.AssignedUser)
            .Include(t => t.Comments)
            .ThenInclude(c => c.User)
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByColumnAsync(Guid columnId)
    {
        return await _dbSet
            .Include(t => t.AssignedUser)
            .Where(t => t.ColumnId == columnId)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(Guid boardId)
    {
        return await _dbSet
            .Include(t => t.Column)
            .Include(t => t.AssignedUser)
            .Where(t => t.Column.BoardId == boardId)
            .OrderBy(t => t.Column.Order)
            .ThenBy(t => t.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAssignedToUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.Column)
            .ThenInclude(c => c.Board)
            .Where(t => t.AssignedUserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Guid boardId, Priority priority)
    {
        return await _dbSet
            .Include(t => t.Column)
            .Where(t => t.Column.BoardId == boardId && t.Priority == priority)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByDueDateRangeAsync(Guid boardId, DateTime from, DateTime to)
    {
        return await _dbSet
            .Include(t => t.Column)
            .Where(t => t.Column.BoardId == boardId &&
                        t.DueDate.HasValue &&
                        t.DueDate.Value >= from &&
                        t.DueDate.Value <= to)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<int> GetMaxOrderAsync(Guid columnId)
    {
        return await _dbSet
            .Where(t => t.ColumnId == columnId)
            .MaxAsync(t => (int?)t.Order) ?? 0;
    }

    public async Task<int> GetTaskCountByColumnAsync(Guid columnId)
    {
        return await _dbSet
            .CountAsync(t => t.ColumnId == columnId);
    }

    public async Task<int> GetTaskCountByBoardAsync(Guid boardId)
    {
        return await _dbSet
            .CountAsync(t => t.Column.BoardId == boardId);
    }

    public async Task<IEnumerable<TaskItem>> SearchTasksAsync(Guid boardId, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetTasksByBoardAsync(boardId);

        searchTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(t => t.Column)
            .Include(t => t.AssignedUser)
            .Where(t => t.Column.BoardId == boardId &&
                        (t.Title.ToLower().Contains(searchTerm) ||
                         t.Description.ToLower().Contains(searchTerm) ||
                         (t.AssignedUser != null && t.AssignedUser.Username.ToLower().Contains(searchTerm))))
            .OrderBy(t => t.Column.Order)
            .ThenBy(t => t.Order)
            .ToListAsync();
    }

    public async Task ReorderTasksAsync(Guid columnId, int fromOrder, int toOrder)
    {
        var tasks = await _dbSet
            .Where(t => t.ColumnId == columnId)
            .OrderBy(t => t.Order)
            .ToListAsync();

        // پیدا کردن ایندکس‌ها
        var fromIndex = tasks.FindIndex(t => t.Order == fromOrder);
        var toIndex = tasks.FindIndex(t => t.Order == toOrder);

        if (fromIndex == -1 || toIndex == -1)
            return;

        // جابجایی
        var item = tasks[fromIndex];
        tasks.RemoveAt(fromIndex);
        tasks.Insert(toIndex, item);

        // به‌روزرسانی Order
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Reorder(i + 1);
        }

        _dbSet.UpdateRange(tasks);
    }
}