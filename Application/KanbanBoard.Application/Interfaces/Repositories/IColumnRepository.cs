using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IColumnRepository : IRepository<BoardColumn>
{

    Task<BoardColumn?> GetColumnWithTasksAsync(Guid columnId);


    Task<IEnumerable<BoardColumn>> GetColumnsByBoardWithTasksAsync(Guid boardId);


    Task<IEnumerable<BoardColumn>> GetColumnsByBoardAsync(Guid boardId);


    Task<int> GetMaxOrderAsync(Guid boardId);


    Task ReorderColumnsAsync(Guid boardId, int oldOrder, int newOrder);

    //Task<IEnumerable<BoardColumn>> GetDefaultColumnsAsync(Guid boardId);
}