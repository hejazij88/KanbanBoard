using KanbanBoard.Application.Features.Column.Commands;
using KanbanBoard.Application.Features.Column.Queries;
using KanbanBoard.Application.Features.Task.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColumnController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColumnController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetColumnsByBoard(Guid boardId)
        {
            var query = new GetColumnsByBoardQuery { BoardId = boardId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("board/{boardId}")]
        public async Task<IActionResult> CreateColumn(Guid boardId, [FromBody] CreateColumnCommand command)
        {
            command.BoardId = boardId;
            try
            {
                var result = await _mediator.Send(command);
                //return CreatedAtAction(nameof(GetColumnById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
