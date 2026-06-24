using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardsByWorkspaceQueryHandler : IRequestHandler<GetBoardsByWorkspaceQuery, List<BoardDto>>
{
    public async Task<List<BoardDto>> Handle(GetBoardsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}