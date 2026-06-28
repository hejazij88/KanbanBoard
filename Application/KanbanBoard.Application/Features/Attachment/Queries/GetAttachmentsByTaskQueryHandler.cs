using AutoMapper;
using KanbanBoard.Application.DTOs.Attachment;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Attachment.Queries;

public class GetAttachmentsByTaskQueryHandler:IRequestHandler<GetAttachmentsByTaskQuery,List<AttachmentDto>>
{
    private readonly IAttachmentRepository _attachmentRepo;
    private readonly IMapper _mapper;

    public GetAttachmentsByTaskQueryHandler(IAttachmentRepository attachmentRepo, IMapper mapper)
    {
        _attachmentRepo = attachmentRepo;
        _mapper = mapper;
    }

    public async Task<List<AttachmentDto>> Handle(GetAttachmentsByTaskQuery request, CancellationToken cancellationToken)
    {
        var attachments = await _attachmentRepo.GetAttachmentsByTaskAsync(request.TaskId);
        return _mapper.Map<List<AttachmentDto>>(attachments);
    }
}