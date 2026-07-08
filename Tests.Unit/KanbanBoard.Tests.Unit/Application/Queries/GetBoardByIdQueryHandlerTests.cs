using AutoMapper;
using FluentAssertions;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.Features.Board.Queries;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using Moq;

namespace KanbanBoard.Tests.Unit.Application.Queries;

public class GetBoardByIdQueryHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBoardByIdQueryHandler _handler;

    public GetBoardByIdQueryHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetBoardByIdQueryHandler(_boardRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingBoard_ShouldReturnBoardDto()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var query = new GetBoardByIdQuery { BoardId = boardId };

        var board = new Board("Test Board", Guid.NewGuid());
        var boardDto = new BoardDto { Id = board.Id, Title = "Test Board" };

        _boardRepoMock.Setup(r => r.GetBoardWithColumnsAndTasksAsync(boardId))
            .ReturnsAsync(board);
        _mapperMock.Setup(m => m.Map<BoardDto>(board))
            .Returns(boardDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(board.Id);
        result.Title.Should().Be("Test Board");
        _boardRepoMock.Verify(r => r.GetBoardWithColumnsAndTasksAsync(boardId), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentBoard_ShouldThrowException()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var query = new GetBoardByIdQuery { BoardId = boardId };

        _boardRepoMock.Setup(r => r.GetBoardWithColumnsAndTasksAsync(boardId))
            .ReturnsAsync((Board?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(query, CancellationToken.None)
        );
        exception.Message.Should().Contain("Board not found");
    }
}