using System.Security.Cryptography;
using System.Text;
using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Application.Interfaces.Services;
using MediatR;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = (await _userRepo.FindAsync(u => u.Email == request.LogInDto.Email)).FirstOrDefault();
        if (user == null || !VerifyPassword(request.LogInDto.Password, user.PasswordHash))
            throw new Exception("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepo.SaveChangesAsync();

        return new AuthResponseDto
        {
            Id = user.Id,
            Username = user.FullName,
            Email = user.Email,
            Token = token,
            RefreshToken = refreshToken
        };
        return null;
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedHash = Convert.ToBase64String(hash);
        return computedHash == storedHash;
    }
}