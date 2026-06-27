using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand, bool>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteWorkspaceCommandHandler(IWorkspaceRepository workspaceRepo, IHttpContextAccessor httpContextAccessor)
    {
        _workspaceRepo = workspaceRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepo.GetByIdAsync(request.Id);
        if (workspace == null)
            throw new Exception("Workspace not found.");

        var userId = GetCurrentUserId();
        if (workspace.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the owner can delete the workspace.");

        _workspaceRepo.DeleteAsync(workspace);
        await _workspaceRepo.SaveChangesAsync();
        return true;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}