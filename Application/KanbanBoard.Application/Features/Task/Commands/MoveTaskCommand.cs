using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class MoveTaskCommand : IRequest<bool>
{
    public MoveTaskDto MoveTaskDto { get; set; } = null!;
}