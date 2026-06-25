using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class CreateTaskCommand:IRequest<TaskDto>
{
    public Guid ColumnId { get; set; }
    public CreateTaskDto TaskDto { get; set; } = null!;

}