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

        // اعمال تمام تنظیمات در یک متد کمکی
        ConfigureEntities(modelBuilder);
    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
    {
        // 1. تنظیمات User
        ConfigureUser(modelBuilder);

        // 2. تنظیمات Workspace
        ConfigureWorkspace(modelBuilder);

        // 3. تنظیمات WorkspaceMember
        ConfigureWorkspaceMember(modelBuilder);

        // 4. تنظیمات Board
        ConfigureBoard(modelBuilder);

        // 5. تنظیمات BoardColumn
        ConfigureBoardColumn(modelBuilder);

        // 6. تنظیمات TaskItem
        ConfigureTaskItem(modelBuilder);

        // 7. تنظیمات Comment
        ConfigureComment(modelBuilder);

        // 8. تنظیمات Attachment
        ConfigureAttachment(modelBuilder);

        // 9. اعمال Value Conversions برای Enums
        ApplyEnumConversions(modelBuilder);
    }

    #region Entity Configurations

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Table name
            entity.ToTable("Users");

            // Primary Key
            entity.HasKey(u => u.Id);

            // Properties
            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.RefreshToken)
                .HasMaxLength(512)
                .IsRequired(false);

            entity.Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);

            // Indexes
            entity.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            entity.HasIndex(u => u.FullName)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");

            // Shadow property for tracking
            entity.Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property<DateTime?>("UpdatedAt")
                .IsRequired(false);

            // Ignore navigation collections (EF Core will manage them)
            entity.HasMany(u => u.Workspaces)
                .WithOne(wm => wm.User)
                .HasForeignKey(wm => wm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.AssignedTasks)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureWorkspace(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workspace>(entity =>
        {
            entity.ToTable("Workspaces");

            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.OwnerId)
                .IsRequired();

            entity.Property(w => w.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            entity.HasIndex(w => w.OwnerId)
                .HasDatabaseName("IX_Workspaces_OwnerId");

            // Relationships
            entity.HasOne(w => w.Owner)
                .WithMany()
                .HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(w => w.Members)
                .WithOne(m => m.Workspace)
                .HasForeignKey(m => m.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(w => w.Boards)
                .WithOne(b => b.Workspace)
                .HasForeignKey(b => b.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureWorkspaceMember(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkspaceMember>(entity =>
        {
            entity.ToTable("WorkspaceMembers");

            entity.HasKey(wm => wm.Id);

            entity.Property(wm => wm.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Member");

            
            // Unique constraint: یک کاربر نمی‌تواند دو بار در یک Workspace عضو شود
            entity.HasIndex(wm => new { wm.WorkspaceId, wm.UserId })
                .IsUnique()
                .HasDatabaseName("IX_WorkspaceMembers_WorkspaceId_UserId");

            // Relationships
            entity.HasOne(wm => wm.Workspace)
                .WithMany(w => w.Members)
                .HasForeignKey(wm => wm.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(wm => wm.User)
                .WithMany(u => u.Workspaces)
                .HasForeignKey(wm => wm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureBoard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.ToTable("Boards");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            entity.HasIndex(b => b.WorkspaceId)
                .HasDatabaseName("IX_Boards_WorkspaceId");

            // Relationships
            entity.HasOne(b => b.Workspace)
                .WithMany(w => w.Boards)
                .HasForeignKey(b => b.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(b => b.Columns)
                .WithOne(c => c.Board)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureBoardColumn(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardColumn>(entity =>
        {
            entity.ToTable("BoardColumns");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Order)
                .IsRequired();

            // Indexes
            entity.HasIndex(c => new { c.BoardId, c.Order })
                .IsUnique()
                .HasDatabaseName("IX_BoardColumns_BoardId_Order");

            // Relationships
            entity.HasOne(c => c.Board)
                .WithMany(b => b.Columns)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Tasks)
                .WithOne(t => t.Column)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureTaskItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(2000)
                .IsRequired(false);

            entity.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<int>(); // ذخیره Enum به صورت int در دیتابیس

            entity.Property(t => t.Order)
                .IsRequired();

            entity.Property(t => t.DueDate)
                .IsRequired(false);

            entity.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(t => t.UpdatedAt)
                .IsRequired(false);

            // Indexes
            entity.HasIndex(t => t.ColumnId)
                .HasDatabaseName("IX_Tasks_ColumnId");

            entity.HasIndex(t => t.AssignedUserId)
                .HasDatabaseName("IX_Tasks_AssignedUserId");

            entity.HasIndex(t => new { t.ColumnId, t.Order })
                .IsUnique()
                .HasDatabaseName("IX_Tasks_ColumnId_Order");

            entity.HasIndex(t => t.Priority)
                .HasDatabaseName("IX_Tasks_Priority");

            entity.HasIndex(t => t.DueDate)
                .HasDatabaseName("IX_Tasks_DueDate");

            // Relationships
            entity.HasOne(t => t.Column)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(t => t.Attachments)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureComment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            entity.HasIndex(c => c.TaskId)
                .HasDatabaseName("IX_Comments_TaskId");

            entity.HasIndex(c => c.UserId)
                .HasDatabaseName("IX_Comments_UserId");

            // Relationships
            entity.HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAttachment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(a => a.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(a => a.FileSize)
                .IsRequired();


            entity.Property(a => a.UploadedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            entity.HasIndex(a => a.TaskId)
                .HasDatabaseName("IX_Attachments_TaskId");

            // Relationships
            entity.HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    #endregion

    #region Value Conversions

    private void ApplyEnumConversions(ModelBuilder modelBuilder)
    {
        // تبدیل Priority (Enum) به int برای ذخیره در دیتابیس
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Priority)
            .HasConversion<int>();

        // اگر Enum دیگری دارید، اینجا اضافه کنید
        // مثلاً اگر Role در WorkspaceMember Enum است:
        // modelBuilder.Entity<WorkspaceMember>()
        //     .Property(wm => wm.Role)
        //     .HasConversion<string>();
    }

    #endregion

    #region SaveChanges Override (برای AuditableEntity)

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

    #endregion
}