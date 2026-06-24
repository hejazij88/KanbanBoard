using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class RemoveWorkSpaceMemberCommandHandler:IRequestHandler<RemoveWorkspaceMemberCommand,bool>
{
    public async Task<bool> Handle(RemoveWorkspaceMemberCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}