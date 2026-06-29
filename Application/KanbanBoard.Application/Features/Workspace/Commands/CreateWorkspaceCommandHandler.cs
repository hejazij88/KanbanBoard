using System.Security.Claims;
using AutoMapper;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, WorkspaceDto>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var workspace = new Domain.Entities.Workspace(request.WorkspaceDto.Name, userId);

        await _workspaceRepo.AddAsync(workspace);
        await _workspaceRepo.SaveChangesAsync();

        var result = await _workspaceRepo.GetWorkspaceWithMembersAsync(workspace.Id);
        return _mapper.Map<WorkspaceDto>(result);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}