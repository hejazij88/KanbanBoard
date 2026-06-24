using MediatR;

namespace KanbanBoard.Application.Features.Board.Commands;

public class DeleteBoardCommand : IRequest<bool>
{
    public Guid BoardId { get; set; }
}