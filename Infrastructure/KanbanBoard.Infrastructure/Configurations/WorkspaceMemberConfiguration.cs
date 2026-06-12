using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanBoard.Infrastructure.Configurations;

public class WorkspaceMemberConfiguration
    : IEntityTypeConfiguration<WorkspaceMember>
{
    public void Configure(
        EntityTypeBuilder<WorkspaceMember> builder)
    {
        builder.ToTable("WorkspaceMembers");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Workspaces)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Workspace)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.WorkspaceId);

        builder.HasIndex(x =>
                new
                {
                    x.UserId,
                    x.WorkspaceId
                })
            .IsUnique();
    }
}