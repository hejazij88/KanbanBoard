using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class RemoveWorkspaceMemberCommand : IRequest<bool>
{
    public Guid MemberId { get; set; }
    public Guid WorkSpaceId { get; set; }
}