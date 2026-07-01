using KanbanBoard.Application.Features.Task.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("column/{columnId}")]
        public async Task<IActionResult> GetTasksByColumn(Guid columnId)
        {
            var query = new GetTasksByColumnQuery { ColumnId = columnId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var query = new GetTaskByIdQuery { TaskId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
