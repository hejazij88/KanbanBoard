using KanbanBoard.Application.DTOs.Column;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Queries;

public class GetColumnsByBoardQuery:IRequest<List<ColumnDto>>
{
    public Guid BoardId  { get; set; }
}