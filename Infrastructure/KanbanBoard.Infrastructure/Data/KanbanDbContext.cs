using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Data;

public class KanbanDbContext:DbContext
{

    public KanbanDbContext(
        DbContextOptions<KanbanDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();

    public DbSet<Workspace> Workspaces => Set<Workspace>();

    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();

    public DbSet<Board> Boards => Set<Board>();

    public DbSet<BoardColumn> Columns => Set<BoardColumn>();

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(KanbanDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}