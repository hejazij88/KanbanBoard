using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTasksByColumnQueryHandler:IRequestHandler<GetTasksByColumnQuery,List<TaskDto>>
{
    public async Task<List<TaskDto>> Handle(GetTasksByColumnQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}