using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardByIdQueryHandler :IRequestHandler<GetBoardByIdQuery,BoardDto>
{
    public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}