using KanbanBoard.Application.DTOs.Auth;
using MediatR;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    //private readonly IUserRepository _userRepo;
    //private readonly IJwtService _jwtService;

    //public LoginCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    //{
    //    _userRepo = userRepo;
    //    _jwtService = jwtService;
    //}

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //var user = (await _userRepo.FindAsync(u => u.Email == request.LoginDto.Email)).FirstOrDefault();
        //if (user == null || !VerifyPassword(request.LoginDto.Password, user.PasswordHash))
        //    throw new Exception("Invalid email or password.");

        //var token = _jwtService.GenerateToken(user);
        //var refreshToken = _jwtService.GenerateRefreshToken();

        //user.RefreshToken = refreshToken;
        //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        //await _userRepo.SaveChangesAsync();

        //return new AuthResponseDto
        //{
        //    UserId = user.Id,
        //    Username = user.Username,
        //    Email = user.Email,
        //    Token = token,
        //    RefreshToken = refreshToken
        //};
        return null;
    }

    //private bool VerifyPassword(string password, string storedHash)
    //{
    //    using var sha256 = SHA256.Create();
    //    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    //    var computedHash = Convert.ToBase64String(hash);
    //    return computedHash == storedHash;
    //}
}