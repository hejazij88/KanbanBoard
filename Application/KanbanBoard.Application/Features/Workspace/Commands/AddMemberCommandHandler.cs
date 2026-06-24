using KanbanBoard.Application.DTOs.Workspace;
using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, WorkspaceMemberDto>
{
    public async Task<WorkspaceMemberDto> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}