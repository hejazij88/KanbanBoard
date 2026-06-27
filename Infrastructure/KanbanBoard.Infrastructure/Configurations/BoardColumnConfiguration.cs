using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanBoard.Infrastructure.Configurations;

public class BoardColumnConfiguration
    : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(
        EntityTypeBuilder<BoardColumn> builder)
    {
        builder.ToTable("Columns");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.Board)
            .WithMany(x => x.Columns)
            .HasForeignKey(x => x.BoardId);
    }
}