using KanbanBoard.Application.Features.Comment.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetCommentsByTask(Guid taskId)
        {
            var query = new GetCommentsByTaskQuery { TaskId = taskId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
