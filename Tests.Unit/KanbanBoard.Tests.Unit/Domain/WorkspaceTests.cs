using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Tests.Unit.Domain;

public class WorkspaceTests
{
    [Fact]
    public void CreateWorkspace_WithValidData_ShouldCreateWorkspace()
    {
        // Arrange
        var name = "My Workspace";
        var ownerId = Guid.NewGuid();

        // Act
        var workspace = new Workspace(name, ownerId);

        // Assert
        workspace.Id.Should().NotBeEmpty();
        workspace.Name.Should().Be(name);
        workspace.OwnerId.Should().Be(ownerId);
        workspace.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        workspace.Members.Should().BeEmpty();
        workspace.Boards.Should().BeEmpty();
    }

    [Fact]
    public void CreateWorkspace_WithEmptyName_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Workspace("", Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => new Workspace(null!, Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => new Workspace("   ", Guid.NewGuid()));
    }

    [Fact]
    public void SetName_ShouldUpdateWorkspaceName()
    {
        // Arrange
        var workspace = new Workspace("Old Name", Guid.NewGuid());
        var newName = "New Name";

        // Act
        workspace.SetName(newName);

        // Assert
        workspace.Name.Should().Be(newName);
    }

    
    [Fact]
    public void AddMember_ShouldAddUserToWorkspace()
    {
        // Arrange
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var user = new User("user", "email@test.com", "hash");

        // Act
        workspace.AddMember(user, WorkspaceRole.Member);

        // Assert
        workspace.Members.Should().HaveCount(1);
        workspace.Members.First().UserId.Should().Be(user.Id);
        workspace.Members.First().Role.Should().Be(WorkspaceRole.Member);
    }

    [Fact]
    public void AddMember_WhenAlreadyMember_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var user = new User("user", "email@test.com", "hash");
        workspace.AddMember(user, WorkspaceRole.Member);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => workspace.AddMember(user, WorkspaceRole.Admin));
    }

    [Fact]
    public void RemoveMember_ShouldRemoveUserFromWorkspace()
    {
        // Arrange
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var user = new User("user", "email@test.com", "hash");
        workspace.AddMember(user, WorkspaceRole.Member);

        // Act
        workspace.RemoveMember(user.Id);

        // Assert
        workspace.Members.Should().BeEmpty();
    }

    [Fact]
    public void RemoveMember_WhenUserIsOwner_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var workspace = new Workspace("Workspace", ownerId);
        var user = new User("owner", "owner@test.com", "hash");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => workspace.RemoveMember(ownerId));
    }

    [Fact]
    public void CreateBoard_ShouldAddBoardToWorkspace()
    {
        // Arrange
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var boardTitle = "My Board";

        // Act
        var board = workspace.CreateBoard(boardTitle);

        // Assert
        workspace.Boards.Should().HaveCount(1);
        workspace.Boards.First().Title.Should().Be(boardTitle);
        board.WorkspaceId.Should().Be(workspace.Id);
    }
}