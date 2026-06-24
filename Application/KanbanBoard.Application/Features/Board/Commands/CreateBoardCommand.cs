using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Commands;

public class CreateBoardCommand:IRequest<BoardDto>
{
    public CreateBoardDto BoardDto { get; set; }
}