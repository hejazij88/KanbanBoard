using FluentAssertions;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Data;
using KanbanBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Tests.Unit.Infrastructure;

public class WorkspaceRepositoryTests: IDisposable
{
    private readonly KanbanDbContext _context;
    private readonly WorkspaceRepository _repository;

    public WorkspaceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<KanbanDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new KanbanDbContext(options);
        _repository = new WorkspaceRepository(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddWorkspace_ShouldAddToDatabase()
    {
        // Arrange
        var workspace = new Workspace("Test Workspace", Guid.NewGuid());

        // Act
        await _repository.AddAsync(workspace);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == workspace.Id);
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Workspace");
    }

    [Fact]
    public async Task GetWorkspacesByUserAsync_ShouldReturnWorkspacesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var workspace1 = new Workspace("Workspace 1", ownerId);
        var workspace2 = new Workspace("Workspace 2", ownerId);
        var workspace3 = new Workspace("Workspace 3", Guid.NewGuid()); // متعلق به کاربر دیگر

        var user = new User("testuser", "test@email.com", "hash");
        workspace2.AddMember(user, WorkspaceRole.Member);

        await _context.Workspaces.AddRangeAsync(workspace1, workspace2, workspace3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWorkspacesByUserAsync(ownerId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(w => w.Id == workspace1.Id);
        result.Should().Contain(w => w.Id == workspace2.Id);
        result.Should().NotContain(w => w.Id == workspace3.Id);
    }

    [Fact]
    public async Task GetWorkspaceWithMembersAsync_ShouldReturnWorkspaceWithMembers()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);
        var user1 = new User("user1", "user1@email.com", "hash");
        var user2 = new User("user2", "user2@email.com", "hash");

        workspace.AddMember(user1, WorkspaceRole.Member);
        workspace.AddMember(user2, WorkspaceRole.Admin);

        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWorkspaceWithMembersAsync(workspace.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Members.Should().HaveCount(3); // Owner + 2 members
        result.Members.Should().Contain(m => m.Role == WorkspaceRole.Member);
        result.Members.Should().Contain(m => m.Role == WorkspaceRole.Admin);
    }

    [Fact]
    public async Task IsUserMemberAsync_ShouldReturnTrueIfUserIsMember()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);
        var user = new User("member", "member@email.com", "hash");

        workspace.AddMember(user, WorkspaceRole.Member);
        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsUserMemberAsync(workspace.Id, user.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserMemberAsync_ShouldReturnFalseIfUserIsNotMember()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);

        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsUserMemberAsync(workspace.Id, userId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveMemberAsync_ShouldRemoveMemberFromWorkspace()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);
        var user = new User("member", "member@email.com", "hash");

        workspace.AddMember(user, WorkspaceRole.Member);
        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        await _repository.RemoveMemberAsync(workspace.Id, user.Id);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _repository.GetWorkspaceWithMembersAsync(workspace.Id);
        result!.Members.Should().HaveCount(1); // فقط Owner
        result.Members.Should().NotContain(m => m.UserId == user.Id);
    }


    [Fact]
    public async Task GetWorkspacesOwnedByUserAsync_ShouldReturnOwnedWorkspaces()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var workspace1 = new Workspace("Workspace 1", userId);
        var workspace2 = new Workspace("Workspace 2", userId);
        var workspace3 = new Workspace("Workspace 3", Guid.NewGuid());

        await _context.Workspaces.AddRangeAsync(workspace1, workspace2, workspace3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWorkspacesOwnedByUserAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(w => w.Id == workspace1.Id);
        result.Should().Contain(w => w.Id == workspace2.Id);
        result.Should().NotContain(w => w.Id == workspace3.Id);
    }

    [Fact]
    public async Task GetWorkspaceWithBoardsAsync_ShouldReturnWorkspaceWithBoards()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);
        var board1 = new Board("Board 1", workspace.Id);
        var board2 = new Board("Board 2", workspace.Id);

        workspace.CreateBoard("Board 1");
        workspace.CreateBoard("Board 2");

        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWorkspaceWithBoardsAsync(workspace.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Boards.Should().HaveCount(2);
        result.Boards.Should().Contain(b => b.Title == "Board 1");
        result.Boards.Should().Contain(b => b.Title == "Board 2");
    }

    [Fact]
    public async Task GetMemberCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var workspace = new Workspace("Test Workspace", ownerId);
        var user1 = new User("user1", "user1@email.com", "hash");
        var user2 = new User("user2", "user2@email.com", "hash");

        workspace.AddMember(user1, WorkspaceRole.Member);
        workspace.AddMember(user2, WorkspaceRole.Member);

        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMemberCountAsync(workspace.Id);

        // Assert
        result.Should().Be(3); // Owner + 2 members
    }
}