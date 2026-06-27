using AutoMapper;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardByIdQueryHandler :IRequestHandler<GetBoardByIdQuery,BoardDto>
{
    private readonly IBoardRepository _boardRepo;
    private readonly IMapper _mapper;

    public GetBoardByIdQueryHandler(IBoardRepository boardRepo, IMapper mapper)
    {
        _boardRepo = boardRepo;
        _mapper = mapper;
    }

    public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _boardRepo.GetBoardWithColumnsAndTasksAsync(request.BoardId);
        if (board == null)
            throw new Exception("Board not found.");

        return _mapper.Map<BoardDto>(board);
    }
}