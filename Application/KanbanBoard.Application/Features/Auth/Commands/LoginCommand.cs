using KanbanBoard.Application.DTOs.Auth;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public LogInDto LogInDto { get; set; }=null!;
}