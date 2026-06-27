using AutoMapper;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class RemoveWorkSpaceMemberCommandHandler : IRequestHandler<RemoveWorkspaceMemberCommand, bool>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveWorkSpaceMemberCommandHandler(IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _workspaceRepo = workspaceRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(RemoveWorkspaceMemberCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepo.GetByIdAsync(request.WorkSpaceId);
        if (workspace == null)
            throw new Exception("Workspace not found.");

        var currentUserId = GetCurrentUserId();

        if (workspace.OwnerId != currentUserId && !workspace.Members.Any(m => m.UserId == currentUserId && m.Role == WorkspaceRole.Admin))
            throw new UnauthorizedAccessException("Only admins can Remove members.");

        var user = (await _userRepo.FindAsync(u => u.Id == request.MemberId)).FirstOrDefault();
        if (user == null)
            throw new Exception("User not found.");

        if (workspace.Members.Any(m => m.UserId == user.Id))
            throw new Exception("User is already a member.");

        await _workspaceRepo.RemoveMemberAsync(workspace.Id, user.Id);

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