using KanbanBoard.Application.DTOs.Comment;
using MediatR;

namespace KanbanBoard.Application.Features.Comment.Commands;

public class CreateCommentCommand:IRequest<CommentDto>
{
    public CreateCommentDto CreateCommentDto { get; set; }
}