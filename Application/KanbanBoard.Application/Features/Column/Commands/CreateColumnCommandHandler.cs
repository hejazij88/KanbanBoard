using AutoMapper;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, ColumnDto>
{
    private readonly IColumnRepository _columnRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMapper _mapper;

    public CreateColumnCommandHandler(IColumnRepository columnRepo, IBoardRepository boardRepo, IMapper mapper)
    {
        _columnRepo = columnRepo;
        _boardRepo = boardRepo;
        _mapper = mapper;
    }

    public async Task<ColumnDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepo.GetByIdAsync(request.BoardId);
        if (board == null)
            throw new Exception("Board not found.");


        var maxOrder = await _columnRepo.GetMaxOrderAsync(request.BoardId);

        var column = new BoardColumn(request.ColumnDto.Title, request.BoardId,maxOrder+1);


        await _columnRepo.AddAsync(column);
        await _columnRepo.SaveChangesAsync();

        return _mapper.Map<ColumnDto>(column);
    }
}