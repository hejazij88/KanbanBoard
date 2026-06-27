using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Board.Commands;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, bool>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteBoardCommandHandler(IBoardRepository boardRepository, IHttpContextAccessor httpContextAccessor)
    {
        _boardRepository = boardRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId);
        if (board == null)
            throw new Exception("Board not found.");

        var userId = GetCurrentUserId();
        if (board.Workspace.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the owner can delete the Board.");

        _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();
        return true;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}