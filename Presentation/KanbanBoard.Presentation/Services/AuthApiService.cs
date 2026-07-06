using KanbanBoard.Application.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public class AuthApiService : BaseApiService
{
    public AuthApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<AuthApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<AuthResponseDto> LoginAsync(LogInDto loginDto, CancellationToken ct = default)
    {
        return await PostAsync<object, AuthResponseDto>("api/auth/login", new { LoginDto = loginDto }, ct);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
    {
        return await PostAsync<object, AuthResponseDto>("api/auth/register", new { RegisterDto = registerDto }, ct);
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        await PostAsync<object>("api/auth/logout", new { }, ct);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken ct = default)
    {
        return await PostAsync<object, AuthResponseDto>("api/auth/refresh", new { RefreshTokenDto = refreshTokenDto }, ct);
    }
}