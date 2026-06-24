using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Commands;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, BoardDto>
{
    public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}