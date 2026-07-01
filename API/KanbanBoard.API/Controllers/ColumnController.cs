using KanbanBoard.Application.Features.Column.Queries;
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
    }
}
