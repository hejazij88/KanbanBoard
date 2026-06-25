using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Queries;

public class GetColumnsByBoardQueryHandler:IRequestHandler<GetColumnsByBoardQuery,List<ColumnDto>>
{
    public async Task<List<ColumnDto>> Handle(GetColumnsByBoardQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}