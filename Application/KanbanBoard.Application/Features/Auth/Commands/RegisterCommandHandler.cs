using System.Security.Cryptography;
using System.Text;
using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Application.Interfaces.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // بررسی تکراری نبودن ایمیل
        var existing = await _userRepo.FindAsync(u => u.Email == request.RegisterDto.Email);
        if (existing.Any())
            throw new Exception("User with this email already exists.");

        var user = new User(request.RegisterDto.UserName,request.RegisterDto.Email,HashPassword(request.RegisterDto.Password));
        
        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        // تولید توکن
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
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
   
}