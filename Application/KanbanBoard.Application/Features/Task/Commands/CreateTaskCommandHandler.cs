using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class CreateTaskCommandHandler:IRequestHandler<CreateTaskCommand,TaskDto>
{
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}