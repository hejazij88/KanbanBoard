using AutoMapper;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardsByWorkspaceQueryHandler : IRequestHandler<GetBoardsByWorkspaceQuery, List<BoardDto>>
{
    private readonly IBoardRepository _boardRepo;
    private readonly IMapper _mapper;

    public GetBoardsByWorkspaceQueryHandler(IBoardRepository boardRepo, IMapper mapper)
    {
        _boardRepo = boardRepo;
        _mapper = mapper;
    }

    public async Task<List<BoardDto>> Handle(GetBoardsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        var boards = await _boardRepo.GetBoardsByWorkspaceAsync(request.WorkspaceId);
        return _mapper.Map<List<BoardDto>>(boards);
    }
}