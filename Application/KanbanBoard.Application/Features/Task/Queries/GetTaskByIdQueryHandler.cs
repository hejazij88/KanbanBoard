using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTaskByIdQueryHandler:IRequestHandler<GetTaskByIdQuery,TaskDto>
{
    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}