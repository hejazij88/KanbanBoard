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
    }
}
