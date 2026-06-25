using KanbanBoard.Application.DTOs.Attachment;
using MediatR;

namespace KanbanBoard.Application.Features.Attachment.Commands;

public class CreateAttachmentCommandHandler:IRequestHandler<CreateAttachmentCommand,AttachmentDto>
{
    public async Task<AttachmentDto> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}