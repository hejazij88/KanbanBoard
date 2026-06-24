using KanbanBoard.Application.DTOs.Auth;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class RegisterCommand:IRequest<AuthResponseDto>
{
    public RegisterDto RegisterDto { get; set; } = null!;
}