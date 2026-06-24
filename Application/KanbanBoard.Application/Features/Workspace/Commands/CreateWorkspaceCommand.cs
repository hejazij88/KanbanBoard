using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class CreateWorkspaceCommand:IRequest<WorkspaceDto>, IRequest<CreateWorkspaceDto>
{
    public CreateWorkspaceDto WorkspaceDto { get; set; } = null!;
}