using AutoMapper;
using KanbanBoard.Application.DTOs.Comment;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Comment.Queries;

public class GetCommentsByTaskQueryHandler : IRequestHandler<GetCommentsByTaskQuery, List<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    public GetCommentsByTaskQueryHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> Handle(GetCommentsByTaskQuery request, CancellationToken cancellationToken)
    {
        var list = await _commentRepository.GetCommentsByTaskAsync(request.TaskId);
        return _mapper.Map<List<CommentDto>>(list);
    }
}