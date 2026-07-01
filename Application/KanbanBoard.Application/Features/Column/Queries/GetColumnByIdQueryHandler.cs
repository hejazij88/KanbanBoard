using AutoMapper;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Queries;

public class GetColumnByIdQueryHandler : IRequestHandler<GetColumnByIdQuery, ColumnDto>
{
    private readonly IColumnRepository _columnRepo;
    private readonly IMapper _mapper;

    public GetColumnByIdQueryHandler(IColumnRepository columnRepo, IMapper mapper)
    {
        _columnRepo = columnRepo;
        _mapper = mapper;
    }

    public async Task<ColumnDto> Handle(GetColumnByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _columnRepo.GetByIdAsync(request.Id);
        if (board == null)
            throw new Exception("ColumnDto not found.");

        return _mapper.Map<ColumnDto>(board);
    }
}