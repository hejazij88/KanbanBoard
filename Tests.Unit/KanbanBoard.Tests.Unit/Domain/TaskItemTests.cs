using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Tests.Unit.Domain;

public class TaskItemTests
{
    [Fact]
    public void CreateTaskItem_WithValidParameters_ShouldCreateTask()
    {
        // Arrange
        var title = "Test Task";
        var description = "Test Description";
        var priority = Priority.High;
        var columnId = Guid.NewGuid();
        var order = 1;

        // Act
        var task = new TaskItem(title, description, priority, columnId, order);

        // Assert
        task.Id.Should().NotBeEmpty();
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.Priority.Should().Be(priority);
        task.ColumnId.Should().Be(columnId);
        task.Order.Should().Be(order);
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        task.Comments.Should().BeEmpty();
        task.Attachments.Should().BeEmpty();
        task.AssignedUserId.Should().BeNull();
        task.DueDate.Should().BeNull();
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateTaskProperties()
    {
        // Arrange
        var task = new TaskItem("Old Title", "Old Description", Priority.Low, Guid.NewGuid(), 1);
        var newTitle = "New Title";
        var newDescription = "New Description";
        var newPriority = Priority.Critical;
        var newDueDate = DateTime.UtcNow.AddDays(5);

        // Act
        task.UpdateDetails(newTitle, newDescription, newPriority, newDueDate);

        // Assert
        task.Title.Should().Be(newTitle);
        task.Description.Should().Be(newDescription);
        task.Priority.Should().Be(newPriority);
        task.DueDate.Should().Be(newDueDate);
        task.UpdatedAt.Should().NotBeNull();
        task.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AssignToUser_ShouldAssignTaskToUser()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var userId = Guid.NewGuid();

        // Act
        task.AssignToUser(userId);

        // Assert
        task.AssignedUserId.Should().Be(userId);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UnassignUser_ShouldRemoveTaskAssignment()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        task.AssignToUser(Guid.NewGuid());

        // Act
        task.UnassignUser();

        // Assert
        task.AssignedUserId.Should().BeNull();
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void SetDueDate_ShouldUpdateDueDate()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var dueDate = DateTime.UtcNow.AddDays(3);

        // Act
        task.SetDueDate(dueDate);

        // Assert
        task.DueDate.Should().Be(dueDate);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MoveToColumn_ShouldMoveTaskToNewColumnWithNewOrder()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var newColumnId = Guid.NewGuid();
        var newOrder = 5;

        // Act
        task.MoveToColumn(newColumnId, newOrder);

        // Assert
        task.ColumnId.Should().Be(newColumnId);
        task.Order.Should().Be(newOrder);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Reorder_ShouldUpdateTaskOrder()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var newOrder = 10;

        // Act
        task.Reorder(newOrder);

        // Assert
        task.Order.Should().Be(newOrder);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Reorder_WithNegativeOrder_ShouldThrowArgumentException()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => task.Reorder(-1));
    }

    [Fact]
    public void AddComment_ShouldAddCommentToTask()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var comment = new Comment("Test comment", task.Id, Guid.NewGuid());

        // Act
        task.AddComment(comment);

        // Assert
        task.Comments.Should().Contain(comment);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void AddAttachment_ShouldAddAttachmentToTask()
    {
        // Arrange
        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var attachment = new Attachment("file.pdf", "/uploads/file.pdf", 1024, "application/pdf", task.Id);

        // Act
        task.AddAttachment(attachment);

        // Assert
        task.Attachments.Should().Contain(attachment);
        task.UpdatedAt.Should().NotBeNull();
    }
}