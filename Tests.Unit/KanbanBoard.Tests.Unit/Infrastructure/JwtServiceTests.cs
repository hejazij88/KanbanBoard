using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KanbanBoard.Tests.Unit.Infrastructure;

public class JwtServiceTests
{
    private readonly JwtService _jwtService;
    private readonly IConfiguration _configuration;

    public JwtServiceTests()
    {
        var configData = new Dictionary<string, string>
        {
            ["Jwt:Key"] = "ThisIsASecretKeyForJWTTokenWithMinimum32Characters!",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience"
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();
        _jwtService = new JwtService(_configuration);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        // Arrange
        var user = new User("testuser", "test@email.com", "hash");

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        jwt.Claims.Should().Contain(c => c.Type == "sub" && c.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == "email" && c.Value == user.Email);
        jwt.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == user.Username);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnValidRefreshToken()
    {
        // Act
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Assert
        refreshToken.Should().NotBeNullOrEmpty();
        refreshToken.Should().HaveLength(44); // Base64 encoded 32 bytes
    }

    [Fact]
    public void ValidateToken_WithValidToken_ShouldReturnTrue()
    {
        // Arrange
        var user = new User("testuser", "test@email.com", "hash");
        var token = _jwtService.GenerateToken(user);

        // Act
        var result = _jwtService.ValidateToken(token);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateToken_WithInvalidToken_ShouldReturnFalse()
    {
        // Arrange
        var invalidToken = "invalid.token.here";

        // Act
        var result = _jwtService.ValidateToken(invalidToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ShouldExtractClaims()
    {
        // Arrange
        var user = new User("testuser", "test@email.com", "hash");
        var token = _jwtService.GenerateToken(user);

        // Act
        var principal = _jwtService.GetPrincipalFromExpiredToken(token);

        // Assert
        principal.Should().NotBeNull();
        var userIdClaim = principal.FindFirst("sub");
        userIdClaim.Should().NotBeNull();
        userIdClaim!.Value.Should().Be(user.Id.ToString());
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_WithInvalidToken_ShouldThrowException()
    {
        // Arrange
        var invalidToken = "invalid.token.here";

        // Act & Assert
        Assert.Throws<SecurityTokenException>(
            () => _jwtService.GetPrincipalFromExpiredToken(invalidToken)
        );
    }
}