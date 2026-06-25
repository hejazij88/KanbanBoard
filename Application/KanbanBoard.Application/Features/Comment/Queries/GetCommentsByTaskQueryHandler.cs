using KanbanBoard.Application.DTOs.Comment;
using MediatR;

namespace KanbanBoard.Application.Features.Comment.Queries;

public class GetCommentsByTaskQueryHandler : IRequestHandler<GetCommentsByTaskQuery, List<CommentDto>>
{
    public async Task<List<CommentDto>> Handle(GetCommentsByTaskQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}