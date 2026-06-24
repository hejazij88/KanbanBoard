using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class CreateColumnCommandHandler:IRequestHandler<CreateColumnCommand,ColumnDto>
{
    public async Task<ColumnDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}