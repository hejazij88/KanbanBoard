using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class DeleteTaskCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}