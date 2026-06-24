using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class UpdateColumnCommnadHandler:IRequestHandler<UpdateColumnCommnad,ColumnDto>
{
    public async Task<ColumnDto> Handle(UpdateColumnCommnad request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}