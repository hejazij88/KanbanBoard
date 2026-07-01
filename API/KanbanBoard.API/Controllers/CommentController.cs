using KanbanBoard.Application.Features.Comment.Commands;
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


        [HttpPost("task/{taskId}")]
        public async Task<IActionResult> CreateComment(Guid taskId, [FromBody] CreateCommentCommand command)
        {
            command.CreateCommentDto.TaskId = taskId;
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetCommentsByTask), new { taskId = result.TaskId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
