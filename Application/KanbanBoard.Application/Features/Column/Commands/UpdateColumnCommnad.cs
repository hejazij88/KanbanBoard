using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class UpdateColumnCommnad : IRequest<ColumnDto>
{
    public Guid ColumnId{ get; set; }
    public UpdateColumnDto UpdateColumnDto { get; set; }
}