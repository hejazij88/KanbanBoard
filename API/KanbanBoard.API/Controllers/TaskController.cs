using KanbanBoard.Application.Features.Task.Commands;
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


        [HttpPost("column/{columnId}")]
        public async Task<IActionResult> CreateTask(Guid columnId, [FromBody] CreateTaskCommand command)
        {
            command.ColumnId = columnId;
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var command = new DeleteTaskCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("move")]
        public async Task<IActionResult> MoveTask([FromBody] MoveTaskCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
