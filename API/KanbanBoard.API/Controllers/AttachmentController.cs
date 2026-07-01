using KanbanBoard.Application.Features.Attachment.Commands;
using KanbanBoard.Application.Features.Attachment.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttachmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetAttachmentsByTask(Guid taskId)
        {
            var query = new GetAttachmentsByTaskQuery { TaskId = taskId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("task/{taskId}")]
        public async Task<IActionResult> UploadAttachment(Guid taskId, [FromForm] CreateAttachmentCommand command)
        {
            command.TaskId = taskId;
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAttachmentsByTask), new { taskId = result.TaskId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
