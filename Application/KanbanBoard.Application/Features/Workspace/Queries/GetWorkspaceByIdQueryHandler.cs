using AutoMapper;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Queries;

public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;

    public GetWorkspaceByIdQueryHandler(IWorkspaceRepository workspaceRepo, IMapper mapper)
    {
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepo.GetByIdAsync(request.WorkSpaceId); 
        if (workspace == null)
            throw new Exception("Workspace not found.");

        return _mapper.Map<WorkspaceDto>(workspace);
    }
}