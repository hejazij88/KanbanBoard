using Microsoft.AspNetCore.Http;

namespace KanbanBoard.Application.DTOs.Attachment;

public class CreateAttachmentDto
{
    public IFormFile File { get; set; } = null!;
}