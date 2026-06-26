namespace KanbanBoard.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IWorkspaceRepository Workspaces { get; }
    IBoardRepository Boards { get; }
    IColumnRepository Columns { get; }
    ITaskRepository Tasks { get; }
    ICommentRepository Comments { get; }
    IAttachmentRepository Attachments { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}