using KanbanBoard.Application.DTOs.Attachment;
using MediatR;

namespace KanbanBoard.Application.Features.Attachment.Queries;

public class GetAttachmentsByTaskQuery:IRequest<List<AttachmentDto>>
{
    public Guid TaskId { get; set; }
}