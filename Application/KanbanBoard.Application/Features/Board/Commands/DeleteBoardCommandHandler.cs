using MediatR;

namespace KanbanBoard.Application.Features.Board.Commands;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, bool>
{
    public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}