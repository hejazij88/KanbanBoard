using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Queries;

public class GetWorkspacesByUserQuery : IRequest<List<WorkspaceDto>>
{
    public Guid UserId { get; set; }
}