using FluentAssertions;
using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Application.Features.Auth.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Application.Interfaces.Services;
using KanbanBoard.Domain.Entities;
using Moq;
using System.Security.Cryptography;
using System.Text;

namespace KanbanBoard.Tests.Unit.Application.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _handler = new LoginCommandHandler(_userRepoMock.Object, _jwtServiceMock.Object);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var loginDto = new LogInDto() { Email = "test@email.com", Password = "123456" };
        var command = new LoginCommand { LogInDto = loginDto };

        var user = new User("testuser", "test@email.com", HashPassword("123456"));

        _userRepoMock.Setup(r => r.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

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
        result.Username.Should().Be(user.Username);
        result.Email.Should().Be(user.Email);

        user.RefreshToken.Should().Be("refresh_token");
        user.RefreshTokenExpiryTime.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var loginDto = new LogInDto { Email = "test@email.com", Password = "wrongpassword" };
        var command = new LoginCommand { LogInDto = loginDto };

        var user = new User("testuser", "test@email.com", HashPassword("123456"));

        _userRepoMock.Setup(r => r.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        exception.Message.Should().Contain("Invalid");
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowException()
    {
        // Arrange
        var loginDto = new LogInDto { Email = "nonexistent@email.com", Password = "password" };
        var command = new LoginCommand { LogInDto = loginDto };

        _userRepoMock.Setup(r => r.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        exception.Message.Should().Contain("Invalid");
    }
}