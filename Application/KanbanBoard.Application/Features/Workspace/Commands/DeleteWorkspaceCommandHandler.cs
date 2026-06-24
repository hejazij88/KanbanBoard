using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand, bool>
{
    public async Task<bool> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
