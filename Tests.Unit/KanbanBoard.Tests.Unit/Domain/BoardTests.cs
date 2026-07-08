using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Tests.Unit.Domain;

public class BoardTests
{
    [Fact]
    public void CreateBoard_WithValidData_ShouldCreateBoard()
    {
        // Arrange
        var title = "My Board";
        var workspaceId = Guid.NewGuid();
        var description = "Description";

        // Act
        var board = new Board(title, workspaceId, description);

        // Assert
        board.Id.Should().NotBeEmpty();
        board.Title.Should().Be(title);
        board.WorkspaceId.Should().Be(workspaceId);
        board.Description.Should().Be(description);
        board.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        board.Columns.Should().BeEmpty();
    }

    [Fact]
    public void CreateBoard_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Board("", Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => new Board(null!, Guid.NewGuid()));
    }

    [Fact]
    public void SetTitle_ShouldUpdateBoardTitle()
    {
        // Arrange
        var board = new Board("Old Title", Guid.NewGuid());
        var newTitle = "New Title";

        // Act
        board.SetTitle(newTitle);

        // Assert
        board.Title.Should().Be(newTitle);
    }

    [Fact]
    public void AddColumn_ShouldAddColumnToBoard()
    {
        // Arrange
        var board = new Board("Board", Guid.NewGuid());
        var columnTitle = "Todo";

        // Act
        var column = board.AddColumn(columnTitle, 1);

        // Assert
        board.Columns.Should().HaveCount(1);
        board.Columns.First().Title.Should().Be(columnTitle);
        board.Columns.First().Order.Should().Be(1);
        column.BoardId.Should().Be(board.Id);
    }

    [Fact]
    public void RemoveColumn_WhenColumnHasNoTasks_ShouldRemoveColumn()
    {
        // Arrange
        var board = new Board("Board", Guid.NewGuid());
        var column = board.AddColumn("Todo", 1);

        // Act
        board.RemoveColumn(column.Id);

        // Assert
        board.Columns.Should().BeEmpty();
    }

    [Fact]
    public void RemoveColumn_WhenColumnHasTasks_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var board = new Board("Board", Guid.NewGuid());
        var column = board.AddColumn("Todo", 1);
        column.AddTask("Task", "Desc", Priority.Medium);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => board.RemoveColumn(column.Id));
    }

    [Fact]
    public void CreateDefaultColumns_ShouldAddThreeDefaultColumns()
    {
        // Arrange
        var board = new Board("Board", Guid.NewGuid());

        // Act
        board.CreateDefaultColumns();

        // Assert
        board.Columns.Should().HaveCount(3);
        board.Columns.Should().Contain(c => c.Title == "Todo" && c.Order == 1);
        board.Columns.Should().Contain(c => c.Title == "In Progress" && c.Order == 2);
        board.Columns.Should().Contain(c => c.Title == "Done" && c.Order == 3);
    }
}