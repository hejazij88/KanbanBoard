using FluentValidation;
using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto>
{
    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}