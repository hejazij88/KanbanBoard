using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardByIdQuery:IRequest<BoardDto>
{
    public Guid BoardId { get; set; }
}