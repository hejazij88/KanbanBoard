using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class DeleteColumnCommand : IRequest<bool>
{
    public Guid ColumnId { get; set; }
}