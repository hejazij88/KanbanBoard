using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Commands;

public class UpdateBoardCommand : IRequest<BoardDto>
{
    public Guid  BoardId{ get; set; }
    public UpdateBoardDto UpdateBoardDto { get; set; }
}