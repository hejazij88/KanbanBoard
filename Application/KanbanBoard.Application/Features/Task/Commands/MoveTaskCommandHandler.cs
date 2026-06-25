using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, bool>
{
    public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}