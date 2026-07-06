using KanbanBoard.Application.Interfaces.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace KanbanBoard.Presentation.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";
    private readonly AuthenticationState _anonymousState;

    private readonly ILocalStorageService _localStorage;
    private readonly IJwtService _jwtService;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IJwtService jwtService, ILogger<CustomAuthenticationStateProvider> logger)
    {
        _localStorage = localStorage;
        _jwtService = jwtService;
        _logger = logger;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // 1. دریافت توکن از LocalStorage
            var token = await _localStorage.GetItemAsync<string>(TokenKey);

            // 2. اگر توکن وجود نداشت، کاربر ناشناس است
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("No token found in local storage.");
                return _anonymousState;
            }

            // 3. اعتبارسنجی توکن
            if (!_jwtService.ValidateToken(token))
            {
                _logger.LogWarning("Invalid or expired token. Removing from storage.");
                await _localStorage.RemoveItemAsync(TokenKey);
                return _anonymousState;
            }

            // 4. استخراج اطلاعات کاربر از توکن
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                _logger.LogWarning("Failed to extract claims from token.");
                await _localStorage.RemoveItemAsync(TokenKey);
                return _anonymousState;
            }

            // 5. برگرداندن وضعیت احراز هویت
            var state = new AuthenticationState(principal);
            _logger.LogInformation("User authenticated: {Username}",
                principal.Identity.Name ?? "Unknown");

            return state;
        }
        catch (Exception ex)
        {
            // در صورت بروز خطا، کاربر را ناشناس در نظر بگیر
            _logger.LogError(ex, "Error occurred while getting authentication state.");
            return _anonymousState;
        }
    }
    public async Task NotifyUserAuthentication(string token)
    {
        try
        {
            // 1. ذخیره توکن در LocalStorage
            await _localStorage.SetItemAsync(TokenKey, token);

            // 2. استخراج اطلاعات کاربر از توکن
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Invalid token received.");
            }

            // 3. ایجاد وضعیت جدید و اطلاع‌رسانی به برنامه
            var state = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            _logger.LogInformation("User authenticated successfully: {Username}",
                principal.Identity.Name ?? "Unknown");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to authenticate user.");
            throw;
        }
    }

    /// <summary>
    /// اطلاع‌رسانی به برنامه که کاربر خارج شده است
    /// این متد پس از عملیات Logout فراخوانی می‌شود
    /// </summary>
    public async Task NotifyUserLogout()
    {
        try
        {
            // 1. حذف توکن از LocalStorage
            await _localStorage.RemoveItemAsync(TokenKey);

            // 2. اطلاع‌رسانی به برنامه در مورد وضعیت ناشناس
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));

            _logger.LogInformation("User logged out.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout.");
        }
    }

    /// <summary>
    /// دریافت توکن فعلی کاربر (برای استفاده در درخواست‌های HTTP)
    /// </summary>
    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// بررسی اینکه آیا کاربر وارد شده است یا خیر
    /// </summary>
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        return _jwtService.ValidateToken(token);
    }
}