using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class AddMemberCommand : IRequest<WorkspaceMemberDto>
{
    public Guid WorkspaceId { get; set; }
    public AddMemberDto AddMemberDto { get; set; } = null!;
}