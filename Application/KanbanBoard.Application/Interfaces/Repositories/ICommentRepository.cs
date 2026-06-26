namespace KanbanBoard.Application.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Comment?> GetCommentWithUserAsync(Guid commentId);

    Task<IEnumerable<Comment>> GetCommentsByTaskWithUserAsync(Guid taskId);

    Task<IEnumerable<Comment>> GetCommentsByTaskAsync(Guid taskId);

    Task<IEnumerable<Comment>> GetCommentsByUserAsync(Guid userId);

    Task<int> GetCommentCountByTaskAsync(Guid taskId);

    Task DeleteCommentsByTaskAsync(Guid taskId);
}