using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanBoard.Infrastructure.Configurations;

public class BoardConfiguration
    : IEntityTypeConfiguration<Board>
{
    public void Configure(
        EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(x => x.Workspace)
            .WithMany(x => x.Boards)
            .HasForeignKey(x => x.WorkspaceId);
    }
}