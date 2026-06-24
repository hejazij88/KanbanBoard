using KanbanBoard.Application.DTOs.Board;
using MediatR;

namespace KanbanBoard.Application.Features.Board.Queries;

public class GetBoardsByWorkspaceQuery : IRequest<List<BoardDto>>
{
    public Guid WorkspaceId { get; set; }
}