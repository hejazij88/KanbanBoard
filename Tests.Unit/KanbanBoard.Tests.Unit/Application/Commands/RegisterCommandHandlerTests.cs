using FluentAssertions;
using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Application.Features.Auth.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Application.Interfaces.Services;
using KanbanBoard.Domain.Entities;
using Moq;

namespace KanbanBoard.Tests.Unit.Application.Commands;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _handler = new RegisterCommandHandler(_userRepoMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRegistration_ShouldReturnAuthResponse()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            UserName = "testuser",
            Email = "test@email.com",
            Password = "123456"
        };
        var command = new RegisterCommand { RegisterDto = registerDto };

        _userRepoMock.Setup(r => r.FindAsync(u => u.Email == registerDto.Email))
            .ReturnsAsync(new List<User>());

        User? capturedUser = null;
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);

        _userRepoMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult(1));

        _jwtServiceMock.Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns("jwt_token");
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken())
            .Returns("refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("jwt_token");
        result.RefreshToken.Should().Be("refresh_token");
        result.Username.Should().Be(registerDto.UserName);
        result.Email.Should().Be(registerDto.Email);

        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));

        capturedUser.Should().NotBeNull();
        capturedUser!.Username.Should().Be(registerDto.UserName);
        capturedUser.Email.Should().Be(registerDto.Email);
        capturedUser.PasswordHash.Should().NotBeNullOrEmpty();
        capturedUser.PasswordHash.Should().NotBe(registerDto.Password); // Hashed
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            UserName = "testuser",
            Email = "existing@email.com",
            Password = "123456"
        };
        var command = new RegisterCommand { RegisterDto = registerDto };

        var existingUser = new User("existing", "existing@email.com", "hash");
        _userRepoMock.Setup(r => r.FindAsync(u => u.Email == registerDto.Email))
            .ReturnsAsync(new List<User> { existingUser });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        exception.Message.Should().Contain("already exists");
    }

}