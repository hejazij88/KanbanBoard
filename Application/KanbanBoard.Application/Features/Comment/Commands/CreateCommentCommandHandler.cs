using KanbanBoard.Application.DTOs.Comment;
using MediatR;

namespace KanbanBoard.Application.Features.Comment.Commands;

public class CreateCommentCommandHandler:IRequestHandler<CreateCommentCommand,CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}