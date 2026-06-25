using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTaskByIdQuery:IRequest<TaskDto>
{
    public Guid TaskId { get; set; }
}