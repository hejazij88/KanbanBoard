using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceDto>
{
    public async Task<CreateWorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}