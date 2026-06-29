using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class UpdateWorkspaceCommand : IRequest<WorkspaceDto>
{
    public Guid WorkspaceId { get; set; }
    public UpdateWorkspaceDto UpdateWorkspaceDto { get; set; }
}