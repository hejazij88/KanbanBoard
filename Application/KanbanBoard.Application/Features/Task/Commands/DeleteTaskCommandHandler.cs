using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMediator _mediator;
    public DeleteTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator)
    {
        _taskRepository = taskRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
            throw new Exception("Task not found.");


        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();
        return true;
    }
}