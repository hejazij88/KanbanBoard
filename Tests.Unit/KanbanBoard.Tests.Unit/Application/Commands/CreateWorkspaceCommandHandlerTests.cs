using AutoMapper;
using FluentAssertions;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace KanbanBoard.Tests.Unit.Application.Commands;

public class CreateWorkspaceCommandHandlerTests
{
    private readonly Mock<IWorkspaceRepository> _workspaceRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateWorkspaceCommandHandler _handler;

    public CreateWorkspaceCommandHandlerTests()
    {
        _workspaceRepoMock = new Mock<IWorkspaceRepository>();
        _mapperMock = new Mock<IMapper>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _handler = new CreateWorkspaceCommandHandler(
            _workspaceRepoMock.Object,
            _mapperMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    private void SetupHttpContext(Guid userId)
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateWorkspace()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var workspaceId = Guid.NewGuid();
        var workspaceName = "Test Workspace";
        var dto = new CreateWorkspaceDto { Name = workspaceName };
        var command = new CreateWorkspaceCommand { WorkspaceDto = dto };

        SetupHttpContext(userId);

        var workspace = new Workspace(workspaceName, userId);
        var workspaceDto = new WorkspaceDto { Id = workspaceId, Name = workspaceName };

        _mapperMock.Setup(m => m.Map<Workspace>(dto))
            .Returns(workspace);
        _workspaceRepoMock.Setup(r => r.AddAsync(workspace))
            .Returns(Task.CompletedTask);
        _workspaceRepoMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult(1));
        _workspaceRepoMock.Setup(r => r.GetWorkspaceWithMembersAsync(workspace.Id))
            .ReturnsAsync(workspace);
        _mapperMock.Setup(m => m.Map<WorkspaceDto>(It.IsAny<Workspace>()))
            .Returns(workspaceDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(workspaceId);
        result.Name.Should().Be(workspaceName);
        _workspaceRepoMock.Verify(r => r.AddAsync(workspace), Times.Once);
        _workspaceRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutAuthenticatedUser_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new CreateWorkspaceCommand
        {
            WorkspaceDto = new CreateWorkspaceDto { Name = "Test" }
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenWorkspaceCreationFails_ShouldNotReturnDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateWorkspaceCommand
        {
            WorkspaceDto = new CreateWorkspaceDto { Name = "Test" }
        };

        SetupHttpContext(userId);

        _mapperMock.Setup(m => m.Map<Workspace>(It.IsAny<CreateWorkspaceDto>()))
            .Returns(new Workspace("Test", userId));
        _workspaceRepoMock.Setup(r => r.AddAsync(It.IsAny<Workspace>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}