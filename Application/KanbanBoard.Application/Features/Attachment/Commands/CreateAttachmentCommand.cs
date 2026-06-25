using KanbanBoard.Application.DTOs.Attachment;
using MediatR;

namespace KanbanBoard.Application.Features.Attachment.Commands;

public class CreateAttachmentCommand: IRequest<AttachmentDto>
{
    public Guid TaskId { get; set; }
    public CreateAttachmentDto AttachmentDto { get; set; } = null!;
}