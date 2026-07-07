using KanbanBoard.Application.DTOs.Attachment;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace KanbanBoard.Presentation.Services;

public class AttachmentApiService : BaseApiService
{
    public AttachmentApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authProvider,
        ILogger<AttachmentApiService> logger)
        : base(httpClient, authProvider, logger)
    {
    }

    public async Task<List<AttachmentDto>> GetAttachmentsByTaskAsync(Guid taskId, CancellationToken ct = default)
    {
        return await GetAsync<List<AttachmentDto>>($"api/attachments/task/{taskId}", ct) ?? new();
    }

    public async Task<AttachmentDto> UploadAttachmentAsync(Guid taskId, CreateAttachmentDto dto, CancellationToken ct = default)
    {
        using var formData = new MultipartFormDataContent();
        var fileContent = new StreamContent(dto.File.OpenReadStream());
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.File.ContentType);
        formData.Add(fileContent, "file", dto.File.FileName);

        var request = new HttpRequestMessage(HttpMethod.Post, $"api/attachments/task/{taskId}")
        {
            Content = formData
        };
        await AddAuthorizationHeaderAsync(request);

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response);
        }

        var result = await response.Content.ReadFromJsonAsync<AttachmentDto>(cancellationToken: ct);
        return result ?? throw new InvalidOperationException("Failed to deserialize attachment response");
    }

    public async Task DeleteAttachmentAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/attachments/{id}", ct);
    }


    public async Task<AttachmentDto> UploadAttachmentAsync(Guid taskId, IBrowserFile file, CancellationToken ct = default)
    {
        // محدودیت حجم فایل (مثلاً 10 مگابایت)
        const long maxFileSize = 10 * 1024 * 1024;

        using var stream = file.OpenReadStream(maxFileSize);
        using var formData = new MultipartFormDataContent();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        formData.Add(fileContent, "file", file.Name);

        var request = new HttpRequestMessage(HttpMethod.Post, $"api/attachments/task/{taskId}")
        {
            Content = formData
        };
        await AddAuthorizationHeaderAsync(request);

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
            await HandleErrorResponseAsync(response);

        return await response.Content.ReadFromJsonAsync<AttachmentDto>(ct)
               ?? throw new InvalidOperationException("Failed to deserialize attachment response");
    }

}