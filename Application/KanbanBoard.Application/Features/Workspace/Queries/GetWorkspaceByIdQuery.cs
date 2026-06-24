using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Queries;

public class GetWorkspaceByIdQuery:IRequest<WorkspaceDto>
{
    public Guid WorkSpaceId { get; set; }
}