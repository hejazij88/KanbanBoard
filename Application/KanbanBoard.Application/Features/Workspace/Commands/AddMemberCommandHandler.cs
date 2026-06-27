using AutoMapper;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, WorkspaceMemberDto>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddMemberCommandHandler(IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _workspaceRepo = workspaceRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WorkspaceMemberDto> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepo.GetByIdAsync(request.WorkspaceId);
        if (workspace == null)
            throw new Exception("Workspace not found.");

        var currentUserId = GetCurrentUserId();
        // فقط Admin یا Owner می‌تواند عضو اضافه کند
        if (workspace.OwnerId != currentUserId && !workspace.Members.Any(m => m.UserId == currentUserId && m.Role == WorkspaceRole.Admin))
            throw new UnauthorizedAccessException("Only admins can add members.");

        var user = (await _userRepo.FindAsync(u => u.Email == request.AddMemberDto.Email)).FirstOrDefault();
        if (user == null)
            throw new Exception("User not found.");

        if (workspace.Members.Any(m => m.UserId == user.Id))
            throw new Exception("User is already a member.");

        var member = new WorkspaceMember(user.Id, workspace.Id, WorkspaceRole.Member);

        workspace.Members.Add(member);
        await _workspaceRepo.SaveChangesAsync();

        return _mapper.Map<WorkspaceMemberDto>(member);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}