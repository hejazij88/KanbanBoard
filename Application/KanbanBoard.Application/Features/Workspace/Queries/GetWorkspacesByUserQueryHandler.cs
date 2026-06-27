using AutoMapper;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Queries;

public class GetWorkspacesByUserQueryHandler : IRequestHandler<GetWorkspacesByUserQuery, List<WorkspaceDto>>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;

    public GetWorkspacesByUserQueryHandler(IWorkspaceRepository workspaceRepo, IMapper mapper)
    {
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
    }

    public async Task<List<WorkspaceDto>> Handle(GetWorkspacesByUserQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _workspaceRepo.GetWorkspacesByUserAsync(request.UserId);
        return _mapper.Map<List<WorkspaceDto>>(workspaces);
    }
}