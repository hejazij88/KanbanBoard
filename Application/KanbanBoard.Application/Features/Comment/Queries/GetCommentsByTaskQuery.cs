using KanbanBoard.Application.DTOs.Comment;
using MediatR;

namespace KanbanBoard.Application.Features.Comment.Queries;

public class GetCommentsByTaskQuery:IRequest<List<CommentDto>>
{
    public Guid TaskId { get; set; }
}