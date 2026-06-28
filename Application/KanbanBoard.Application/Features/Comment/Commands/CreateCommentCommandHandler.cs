using AutoMapper;
using KanbanBoard.Application.DTOs.Comment;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Comment.Commands;

public class CreateCommentCommandHandler:IRequestHandler<CreateCommentCommand,CommentDto>
{
    private readonly ICommentRepository _commentRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCommentCommandHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, ITaskRepository taskRepo, ICommentRepository commentRepo)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskRepo = taskRepo;
        _commentRepo = commentRepo;
    }

    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.CreateCommentDto.TaskId);
        if (task == null)
            throw new Exception("Task not found.");

        var userId = GetCurrentUserId();

        var comment = new Domain.Entities.Comment(request.CreateCommentDto.Content,userId,request.CreateCommentDto.TaskId);

        await _commentRepo.AddAsync(comment);
        await _commentRepo.SaveChangesAsync();

        var fullComment = await _commentRepo.GetCommentWithUserAsync(comment.Id);
        return _mapper.Map<CommentDto>(fullComment);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}