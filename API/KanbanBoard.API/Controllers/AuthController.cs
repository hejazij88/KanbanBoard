using KanbanBoard.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private void SetTokenCookies(string token, string refreshToken)
        {
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            try
            {
                var result = await _mediator.Send(registerCommand);
                SetTokenCookies(result.Token, result.RefreshToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
