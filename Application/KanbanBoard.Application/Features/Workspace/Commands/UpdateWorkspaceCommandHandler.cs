using AutoMapper;
using FluentValidation;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepo.GetWorkspaceWithMembersAsync(request.WorkspaceId);
        if (workspace == null)
            throw new Exception("Workspace not found.");

        var userId = GetCurrentUserId();
        if (workspace.OwnerId != userId && !workspace.Members.Any(m => m.UserId == userId && m.Role == WorkspaceRole.Admin))
            throw new UnauthorizedAccessException("You don't have permission to update this workspace.");

        workspace.SetName(request.UpdateWorkspaceDto.Name);

        _workspaceRepo.Update(workspace);
        await _workspaceRepo.SaveChangesAsync();

        return _mapper.Map<WorkspaceDto>(workspace);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}