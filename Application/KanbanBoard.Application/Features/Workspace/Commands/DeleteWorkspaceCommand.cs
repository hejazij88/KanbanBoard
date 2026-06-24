using MediatR;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class DeleteWorkspaceCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}