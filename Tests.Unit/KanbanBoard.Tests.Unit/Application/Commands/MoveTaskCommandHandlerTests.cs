using FluentAssertions;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Application.Features.Task.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using Moq;

namespace KanbanBoard.Tests.Unit.Application.Commands;

public class MoveTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<IColumnRepository> _columnRepoMock;
    private readonly MoveTaskCommandHandler _handler;

    public MoveTaskCommandHandlerTests()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _columnRepoMock = new Mock<IColumnRepository>();
        _handler = new MoveTaskCommandHandler(_taskRepoMock.Object, _columnRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldMoveTaskToNewColumn()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var targetColumnId = Guid.NewGuid();
        var dto = new MoveTaskDto { TaskId = taskId, TargetColumnId = targetColumnId };
        var command = new MoveTaskCommand { MoveTaskDto = dto };

        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);
        var targetColumn = new BoardColumn("In Progress", Guid.NewGuid(), 1);

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(task);
        _columnRepoMock.Setup(r => r.GetByIdAsync(targetColumnId))
            .ReturnsAsync(targetColumn);
        _taskRepoMock.Setup(r => r.GetMaxOrderAsync(targetColumnId))
            .ReturnsAsync(2);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        task.ColumnId.Should().Be(targetColumnId);
        task.Order.Should().Be(3); // maxOrder + 1
        _taskRepoMock.Verify(r => r.Update(task), Times.Once);
        _taskRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReorderTaskInSameColumn()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        var dto = new MoveTaskDto { TaskId = taskId, TargetColumnId = columnId, NewOrder = 5 };
        var command = new MoveTaskCommand { MoveTaskDto = dto };

        var task = new TaskItem("Task", "Desc", Priority.Medium, columnId, 3);
        var targetColumn = new BoardColumn("Todo", Guid.NewGuid(), 1);

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(task);
        _columnRepoMock.Setup(r => r.GetByIdAsync(columnId))
            .ReturnsAsync(targetColumn);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        task.Order.Should().Be(5);
        task.ColumnId.Should().Be(columnId);
        _taskRepoMock.Verify(r => r.Update(task), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentTask_ShouldThrowException()
    {
        // Arrange
        var dto = new MoveTaskDto { TaskId = Guid.NewGuid(), TargetColumnId = Guid.NewGuid() };
        var command = new MoveTaskCommand { MoveTaskDto = dto };

        _taskRepoMock.Setup(r => r.GetByIdAsync(dto.TaskId))
            .ReturnsAsync((TaskItem?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        exception.Message.Should().Contain("Task not found");
    }

    [Fact]
    public async Task Handle_WithNonExistentColumn_ShouldThrowException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var dto = new MoveTaskDto { TaskId = taskId, TargetColumnId = Guid.NewGuid() };
        var command = new MoveTaskCommand { MoveTaskDto = dto };

        var task = new TaskItem("Task", "Desc", Priority.Medium, Guid.NewGuid(), 1);

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(task);
        _columnRepoMock.Setup(r => r.GetByIdAsync(dto.TargetColumnId))
            .ReturnsAsync((BoardColumn?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        exception.Message.Should().Contain("Target column not found");
    }
}