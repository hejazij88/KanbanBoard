namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IBoardRepository : IRepository<Board>
{
    Task<Board?> GetBoardWithColumnsAndTasksAsync(Guid boardId);

    Task<Board?> GetBoardWithColumnsAsync(Guid boardId);
    
    Task<IEnumerable<Board>> GetBoardsByWorkspaceAsync(Guid workspaceId);
    
    Task<IEnumerable<Board>> GetBoardsByUserAsync(Guid userId);
    
    Task<int> GetBoardCountAsync(Guid workspaceId);
    
    Task<bool> IsTitleExistsInWorkspaceAsync(Guid workspaceId, string title, Guid? excludeBoardId = null);
}