using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace KanbanBoard.Presentation.Services;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly AuthenticationStateProvider _authProvider;
    protected readonly ILogger _logger;

    protected BaseApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger logger)
    {
        _httpClient = httpClient;
        _authProvider = authProvider;
        _logger = logger;
    }

    #region Protected Helper Methods

    /// <summary>
    /// دریافت توکن JWT از وضعیت احراز هویت
    /// </summary>
    protected virtual async Task<string?> GetTokenAsync()
    {
        try
        {
            if (_authProvider is CustomAuthenticationStateProvider customProvider)
            {
                return await customProvider.GetTokenAsync();
            }

            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var tokenClaim = user.FindFirst("access_token") ?? user.FindFirst("token");
            return tokenClaim?.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting token");
            return null;
        }
    }

    /// <summary>
    /// اضافه کردن هدر Authorization به درخواست
    /// </summary>
    protected virtual async Task AddAuthorizationHeaderAsync(HttpRequestMessage request)
    {
        var token = await GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// ارسال درخواست و پردازش پاسخ با بازگشت مقدار
    /// </summary>
    protected virtual async Task<T> SendAsync<T>(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        await AddAuthorizationHeaderAsync(request);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize response");
        }

        return result;
    }

    /// <summary>
    /// ارسال درخواست بدون بازگشت مقدار (برای DELETE, PUT, POST بدون خروجی)
    /// </summary>
    protected virtual async Task SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        await AddAuthorizationHeaderAsync(request);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response);
        }
    }

    /// <summary>
    /// متد کمکی برای GET
    /// </summary>
    protected virtual async Task<T> GetAsync<T>(
        string url,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendAsync<T>(request, cancellationToken);
    }

    /// <summary>
    /// متد کمکی برای POST با بازگشت مقدار
    /// </summary>
    protected virtual async Task<TResponse> PostAsync<TRequest, TResponse>(
        string url,
        TRequest data,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(data)
        };
        return await SendAsync<TResponse>(request, cancellationToken);
    }

    /// <summary>
    /// متد کمکی برای POST بدون بازگشت مقدار
    /// </summary>
    protected virtual async Task PostAsync<TRequest>(
        string url,
        TRequest data,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(data)
        };
        await SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// متد کمکی برای PUT
    /// </summary>
    protected virtual async Task PutAsync<TRequest>(
        string url,
        TRequest data,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(data)
        };
        await SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// متد کمکی برای DELETE
    /// </summary>
    protected virtual async Task DeleteAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        await SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// پردازش خطاهای پاسخ
    /// </summary>
    private async Task HandleErrorResponseAsync(HttpResponseMessage response)
    {
        var errorContent = await response.Content.ReadAsStringAsync();
        _logger.LogError("API Error: {StatusCode} - {Error}", response.StatusCode, errorContent);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (_authProvider is CustomAuthenticationStateProvider customProvider)
            {
                await customProvider.NotifyUserLogout();
            }
            throw new UnauthorizedAccessException("Your session has expired. Please login again.");
        }

        throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
    }

    #endregion
}