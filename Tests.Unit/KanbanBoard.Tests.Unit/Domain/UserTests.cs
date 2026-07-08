using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Tests.Unit.Domain;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var username = "john_doe";
        var email = "john@example.com";
        var passwordHash = "hashed_password";

        // Act
        var user = new User(username, email, passwordHash);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.WorkspaceMembers.Should().BeEmpty();
        user.AssignedTasks.Should().BeEmpty();
        user.Comments.Should().BeEmpty();
    }

    [Fact]
    public void CreateUser_WithEmptyUsername_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User("", "email@test.com", "hash"));
        Assert.Throws<ArgumentException>(() => new User(null!, "email@test.com", "hash"));
        Assert.Throws<ArgumentException>(() => new User("   ", "email@test.com", "hash"));
    }

    [Fact]
    public void CreateUser_WithInvalidEmail_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User("user", "invalid-email", "hash"));
        Assert.Throws<ArgumentException>(() => new User("user", "", "hash"));
        Assert.Throws<ArgumentException>(() => new User("user", null!, "hash"));
    }

    [Fact]
    public void SetUsername_ShouldUpdateUsername()
    {
        // Arrange
        var user = new User("oldname", "email@test.com", "hash");
        var newUsername = "newname";

        // Act
        user.SetUsername(newUsername);

        // Assert
        user.Username.Should().Be(newUsername);
    }

    [Fact]
    public void SetEmail_ShouldUpdateEmail()
    {
        // Arrange
        var user = new User("user", "old@email.com", "hash");
        var newEmail = "new@email.com";

        // Act
        user.SetEmail(newEmail);

        // Assert
        user.Email.Should().Be(newEmail);
    }

    [Fact]
    public void SetRefreshToken_ShouldUpdateRefreshToken()
    {
        // Arrange
        var user = new User("user", "email@test.com", "hash");
        var refreshToken = "refresh_token_value";
        var expiryTime = DateTime.UtcNow.AddDays(7);

        // Act
        user.SetRefreshToken(refreshToken, expiryTime);

        // Assert
        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiryTime.Should().Be(expiryTime);
    }

    [Fact]
    public void AddWorkspaceMember_ShouldAddMemberToUser()
    {
        // Arrange
        var user = new User("user", "email@test.com", "hash");
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var member = new WorkspaceMember(workspace, user, WorkspaceRole.Member);

        // Act
        user.AddWorkspaceMember(member);

        // Assert
        user.WorkspaceMembers.Should().Contain(member);
    }

    [Fact]
    public void AddWorkspaceMember_WhenAlreadyMember_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var user = new User("user", "email@test.com", "hash");
        var workspace = new Workspace("Workspace", Guid.NewGuid());
        var member1 = new WorkspaceMember(workspace, user, WorkspaceRole.Member);
        var member2 = new WorkspaceMember(workspace, user, WorkspaceRole.Admin);

        user.AddWorkspaceMember(member1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => user.AddWorkspaceMember(member2));
    }
}