using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Queries;

public class GetWorkspacesByUserQueryHandler : IRequestHandler<GetWorkspacesByUserQuery, List<WorkspaceDto>>
{
    public async Task<List<WorkspaceDto>> Handle(GetWorkspacesByUserQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}