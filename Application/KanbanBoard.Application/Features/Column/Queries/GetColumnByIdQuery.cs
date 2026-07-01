using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Queries;

public class GetColumnByIdQuery:IRequest<ColumnDto>
{
    public Guid Id { get; set; }
}