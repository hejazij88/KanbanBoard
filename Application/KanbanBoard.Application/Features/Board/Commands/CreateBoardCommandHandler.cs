using AutoMapper;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Board.Commands;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, BoardDto>
{
    private readonly IBoardRepository _boardRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateBoardCommandHandler(IBoardRepository boardRepo, IWorkspaceRepository workspaceRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _boardRepo = boardRepo;
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var workspace = await _workspaceRepo.GetWorkspaceWithMembersAsync(request.BoardDto.WorkspaceId);
        if (workspace == null)
            throw new Exception("Workspace not found.");

        if (workspace.OwnerId != userId && !workspace.Members.Any(m => m.UserId == userId))
            throw new UnauthorizedAccessException("You are not a member of this workspace.");

        var board = _mapper.Map<Domain.Entities.Board>(request.BoardDto);

        await _boardRepo.AddAsync(board);
        await _boardRepo.SaveChangesAsync();

        var result = await _boardRepo.GetBoardWithColumnsAndTasksAsync(board.Id);
        return _mapper.Map<BoardDto>(result);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}