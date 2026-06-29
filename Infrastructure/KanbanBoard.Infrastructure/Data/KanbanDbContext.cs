using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Data;

public class KanbanDbContext : DbContext
{
    #region DbSet Properties

    public DbSet<User> Users { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    #endregion

    public KanbanDbContext(DbContextOptions<KanbanDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(u => u.RefreshToken).HasMaxLength(512);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasMany(u => u.WorkspaceMembers).WithOne(w => w.User).HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Workspace
        modelBuilder.Entity<Workspace>(entity =>
        {
            entity.ToTable("Workspaces");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(w => w.Owner).WithMany().HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(w => w.Members).WithOne(m => m.Workspace).HasForeignKey(m => m.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(w => w.Boards).WithOne(b => b.Workspace).HasForeignKey(b => b.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // WorkspaceMember
        modelBuilder.Entity<WorkspaceMember>(entity =>
        {
            entity.ToTable("WorkspaceMembers");
            entity.HasKey(wm => wm.Id);
            entity.Property(wm => wm.Role).HasConversion<string>().HasMaxLength(50);
            entity.HasIndex(wm => new { wm.WorkspaceId, wm.UserId }).IsUnique();
        });

        // Board
        modelBuilder.Entity<Board>(entity =>
        {
            entity.ToTable("Boards");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Description).HasMaxLength(500);
            entity.HasMany(b => b.Columns).WithOne(c => c.Board).HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BoardColumn
        modelBuilder.Entity<BoardColumn>(entity =>
        {
            entity.ToTable("BoardColumns");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(100);
            entity.HasIndex(c => new { c.BoardId, c.Order }).IsUnique();
            entity.HasMany(c => c.TaskItems).WithOne(t => t.Column).HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TaskItem
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(2000);
            entity.Property(t => t.Priority).HasConversion<int>();
            entity.HasIndex(t => new { t.ColumnId, t.Order }).IsUnique();
            entity.HasOne(t => t.AssignedUser).WithMany(u => u.AssignedTasks).HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(t => t.Comments).WithOne(c => c.Task).HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(t => t.Attachments).WithOne(a => a.Task).HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Comment
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Content).IsRequired().HasMaxLength(2000);
        });

        // Attachment
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FileName).IsRequired().HasMaxLength(255);
            entity.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(a => a.ContentType).IsRequired().HasMaxLength(100);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // به‌روزرسانی خودکار فیلدهای CreatedAt و UpdatedAt
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is AuditableEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((AuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                ((AuditableEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}