using KanbanBoard.Application.DTOs.Attachment;
using MediatR;

namespace KanbanBoard.Application.Features.Attachment.Queries;

public class GetAttachmentsByTaskQueryHandler:IRequestHandler<GetAttachmentsByTaskQuery,List<AttachmentDto>>
{
    public async Task<List<AttachmentDto>> Handle(GetAttachmentsByTaskQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}