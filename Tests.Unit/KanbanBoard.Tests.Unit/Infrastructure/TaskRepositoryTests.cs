using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Data;
using KanbanBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Tests.Unit.Infrastructure;

public class TaskRepositoryTests : IDisposable
{
    private readonly KanbanDbContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<KanbanDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new KanbanDbContext(options);
        _repository = new TaskRepository(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private (Workspace workspace, Board board, BoardColumn column) CreateTestData()
    {
        var workspace = new Workspace("Test Workspace", Guid.NewGuid());
        var board = workspace.CreateBoard("Test Board");
        var column = board.AddColumn("Todo", 1);

        _context.Workspaces.Add(workspace);
        _context.SaveChanges();

        return (workspace, board, column);
    }

    [Fact]
    public async Task AddTask_ShouldAddToDatabase()
    {
        // Arrange
        var (_, _, column) = CreateTestData();
        var task = new TaskItem("Test Task", "Description", Priority.Medium, column.Id, 1);

        // Act
        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Task");
        result.Priority.Should().Be(Priority.Medium);
    }

    [Fact]
    public async Task GetTasksByColumnAsync_ShouldReturnTasksInColumn()
    {
        // Arrange
        var (_, _, column) = CreateTestData();
        var task1 = new TaskItem("Task 1", "Desc", Priority.High, column.Id, 1);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Low, column.Id, 2);
        var task3 = new TaskItem("Task 3", "Desc", Priority.Medium, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTasksByColumnAsync(column.Id);

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeInAscendingOrder(t => t.Order);
    }

    [Fact]
    public async Task GetTaskWithDetailsAsync_ShouldReturnTaskWithRelatedData()
    {
        // Arrange
        var (_, _, column) = CreateTestData();
        var user = new User("testuser", "test@email.com", "hash");
        var task = new TaskItem("Test Task", "Desc", Priority.Medium, column.Id, 1);
        task.AssignToUser(user.Id);

        var comment = new Comment("Test comment", task.Id, user.Id);
        task.AddComment(comment);

        var attachment = new Attachment("file.pdf", "/uploads/file.pdf", 1024, "application/pdf", task.Id);
        task.AddAttachment(attachment);

        await _context.Users.AddAsync(user);
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTaskWithDetailsAsync(task.Id);

        // Assert
        result.Should().NotBeNull();
        result!.AssignedUserId.Should().Be(user.Id);
        result.Comments.Should().HaveCount(1);
        result.Attachments.Should().HaveCount(1);
        result.Column.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMaxOrderAsync_ShouldReturnMaxOrder()
    {
        // Arrange
        var (_, _, column) = CreateTestData();
        var task1 = new TaskItem("Task 1", "Desc", Priority.Medium, column.Id, 1);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Medium, column.Id, 5);
        var task3 = new TaskItem("Task 3", "Desc", Priority.Medium, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMaxOrderAsync(column.Id);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public async Task GetTasksByPriorityAsync_ShouldReturnTasksWithPriority()
    {
        // Arrange
        var (_, board, column) = CreateTestData();
        var task1 = new TaskItem("Task 1", "Desc", Priority.High, column.Id, 1);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Low, column.Id, 2);
        var task3 = new TaskItem("Task 3", "Desc", Priority.High, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTasksByPriorityAsync(board.Id, Priority.High);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(t => t.Priority.Should().Be(Priority.High));
    }

    [Fact]
    public async Task SearchTasksAsync_ShouldReturnMatchingTasks()
    {
        // Arrange
        var (_, board, column) = CreateTestData();
        var task1 = new TaskItem("Important Task", "Very important", Priority.High, column.Id, 1);
        var task2 = new TaskItem("Another Task", "Not important", Priority.Medium, column.Id, 2);
        var task3 = new TaskItem("Important Task 2", "Also important", Priority.High, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchTasksAsync(board.Id, "important");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Title.Contains("Important"));
    }

    [Fact]
    public async Task ReorderTasksAsync_ShouldUpdateTaskOrders()
    {
        // Arrange
        var (_, _, column) = CreateTestData();
        var task1 = new TaskItem("Task 1", "Desc", Priority.Medium, column.Id, 1);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Medium, column.Id, 2);
        var task3 = new TaskItem("Task 3", "Desc", Priority.Medium, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        await _repository.ReorderTasksAsync(column.Id, 2, 1);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _repository.GetTasksByColumnAsync(column.Id);
        var orders = result.Select(t => t.Order).ToList();
        orders.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task GetTasksAssignedToUserAsync_ShouldReturnAssignedTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var (_, _, column) = CreateTestData();
        var task1 = new TaskItem("Task 1", "Desc", Priority.Medium, column.Id, 1);
        task1.AssignToUser(userId);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Medium, column.Id, 2);
        task2.AssignToUser(userId);
        var task3 = new TaskItem("Task 3", "Desc", Priority.Medium, column.Id, 3);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTasksAssignedToUserAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(t => t.AssignedUserId.Should().Be(userId));
    }

    [Fact]
    public async Task GetTasksByBoardAsync_ShouldReturnAllTasksInBoard()
    {
        // Arrange
        var (_, board, column) = CreateTestData();
        var column2 = board.AddColumn("In Progress", 2);

        var task1 = new TaskItem("Task 1", "Desc", Priority.Medium, column.Id, 1);
        var task2 = new TaskItem("Task 2", "Desc", Priority.Medium, column.Id, 2);
        var task3 = new TaskItem("Task 3", "Desc", Priority.Medium, column2.Id, 1);

        await _context.Tasks.AddRangeAsync(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTasksByBoardAsync(board.Id);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(t => t.Id == task1.Id);
        result.Should().Contain(t => t.Id == task2.Id);
        result.Should().Contain(t => t.Id == task3.Id);
    }
}