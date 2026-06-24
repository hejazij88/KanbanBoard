using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.DTOs.Comment;
using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class CreateColumnCommand:IRequest<ColumnDto>
{
    public Guid BoardId { get; set; }
    public CreateColumnDto ColumnDto { get; set; }
}