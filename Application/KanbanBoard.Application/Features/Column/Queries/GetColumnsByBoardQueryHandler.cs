using AutoMapper;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Queries;

public class GetColumnsByBoardQueryHandler:IRequestHandler<GetColumnsByBoardQuery,List<ColumnDto>>
{
    private readonly IColumnRepository _columnRepo;
    private readonly IMapper _mapper;

    public GetColumnsByBoardQueryHandler(IColumnRepository columnRepo, IMapper mapper)
    {
        _columnRepo = columnRepo;
        _mapper = mapper;
    }

    public async Task<List<ColumnDto>> Handle(GetColumnsByBoardQuery request, CancellationToken cancellationToken)
    {
        var columns = await _columnRepo.GetColumnsByBoardWithTasksAsync(request.BoardId);
        return _mapper.Map<List<ColumnDto>>(columns.OrderBy(c => c.Order));
    }
}