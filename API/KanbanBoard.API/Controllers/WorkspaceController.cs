using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Features.Workspace.Queries;

namespace KanbanBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkspaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated.");
            return Guid.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWorkSpace()
        {
            try
            {
                var userId = GetCurrentUserId();
                var query = new GetWorkspacesByUserQuery { UserId = userId };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkspaceById(Guid id)
        {
            try
            {
                var query = new GetWorkspaceByIdQuery { WorkSpaceId = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceCommand createWorkspaceCommand)
        {
            try
            {
                var result = await _mediator.Send(createWorkspaceCommand);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateWorkspace(Guid id, [FromBody] UpdateWorkspaceCommand workspaceCommand)
        {

            if (id != workspaceCommand.WorkspaceId)
                return BadRequest("ID Mismatch");
            try
            {
                var result = await _mediator.Send(workspaceCommand);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkspace(Guid id)
        {
            try
            {
                var command = new DeleteWorkspaceCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpPost("{workspaceId}/members")]
        public async Task<IActionResult> AddMember(Guid workspaceId, [FromBody] AddMemberCommand command)
        {
            command.WorkspaceId = workspaceId;
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

        [HttpDelete("{workspaceId}/members/{userId}")]
        public async Task<IActionResult> RemoveMember(Guid workspaceId, Guid userId)
        {
            try
            {
                var command = new RemoveWorkspaceMemberCommand() { WorkSpaceId = workspaceId, MemberId = userId };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
