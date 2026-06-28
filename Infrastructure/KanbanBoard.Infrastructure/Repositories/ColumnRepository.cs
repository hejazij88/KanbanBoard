using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories;

public class ColumnRepository : Repository<BoardColumn>, IColumnRepository
{
    public ColumnRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<BoardColumn?> GetColumnWithTasksAsync(Guid columnId)
    {
        return await _dbSet
            .Include(c => c.Tasks)
            .ThenInclude(t => t.AssignedUser)
            .Include(c => c.Tasks)
            .ThenInclude(t => t.Comments)
            .Include(c => c.Tasks)
            .ThenInclude(t => t.Attachments)
            .FirstOrDefaultAsync(c => c.Id == columnId);
    }

    public async Task<IEnumerable<BoardColumn>> GetColumnsByBoardWithTasksAsync(Guid boardId)
    {
        return await _dbSet
            .Include(c => c.Tasks)
            .ThenInclude(t => t.AssignedUser)
            .Where(c => c.BoardId == boardId)
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<BoardColumn>> GetColumnsByBoardAsync(Guid boardId)
    {
        return await _dbSet
            .Where(c => c.BoardId == boardId)
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<int> GetMaxOrderAsync(Guid boardId)
    {
        return await _dbSet
            .Where(c => c.BoardId == boardId)
            .MaxAsync(c => (int?)c.Order) ?? 0;
    }

    public async Task ReorderColumnsAsync(Guid boardId, int oldOrder, int newOrder)
    {
        var columns = await _dbSet
            .Where(c => c.BoardId == boardId)
            .ToListAsync();

        // مرتب‌سازی بر اساس Order فعلی
        var sorted = columns.OrderBy(c => c.Order).ToList();

        // پیدا کردن ایندکس‌ها
        var fromIndex = sorted.FindIndex(c => c.Order == oldOrder);
        var toIndex = sorted.FindIndex(c => c.Order == newOrder);

        if (fromIndex == -1 || toIndex == -1)
            return;

        // جابجایی
        var item = sorted[fromIndex];
        sorted.RemoveAt(fromIndex);
        sorted.Insert(toIndex, item);

        // به‌روزرسانی Order
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].SetOrder(i + 1);
        }

        _dbSet.UpdateRange(sorted);
    }

    public async Task<IEnumerable<BoardColumn>> GetDefaultColumnsAsync(Guid boardId)
    {
        var defaultColumns = new[]
        {
            new BoardColumn("Todo",1, boardId),
            new BoardColumn("In Progress",2, boardId),
            new BoardColumn("Done", 3, boardId)
        };

        return defaultColumns;
    }
}