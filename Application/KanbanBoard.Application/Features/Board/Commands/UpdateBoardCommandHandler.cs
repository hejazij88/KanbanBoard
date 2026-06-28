using AutoMapper;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Board.Commands;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, BoardDto>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateBoardCommandHandler(IBoardRepository boardRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId);
        if (board == null)
            throw new Exception("Board not found.");

        // بررسی دسترسی (فقط Owner یا Admin)
        var userId = GetCurrentUserId();
        if (board.Workspace.OwnerId != userId && !board.Workspace.Members.Any(m => m.UserId == userId && m.Role == WorkspaceRole.Admin))
            throw new UnauthorizedAccessException("You don't have permission to update this Board.");

        _mapper.Map(request.UpdateBoardDto, board);
        _boardRepository.Update(board);
        await _boardRepository.SaveChangesAsync();

        return _mapper.Map<BoardDto>(board);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}