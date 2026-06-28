using AutoMapper;
using KanbanBoard.Application.DTOs.Attachment;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace KanbanBoard.Application.Features.Attachment.Commands;

public class CreateAttachmentCommandHandler:IRequestHandler<CreateAttachmentCommand,AttachmentDto>
{
    private readonly IAttachmentRepository _attachmentRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public CreateAttachmentCommandHandler(IAttachmentRepository attachmentRepo, ITaskRepository taskRepo, IMapper mapper, IWebHostEnvironment env)
    {
        _attachmentRepo = attachmentRepo;
        _taskRepo = taskRepo;
        _mapper = mapper;
        _env = env;
    }

    public async Task<AttachmentDto> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.TaskId);
        if (task == null)
            throw new Exception("Task not found.");

        var file = request.AttachmentDto.File;
        if (file == null || file.Length == 0)
            throw new Exception("File is required.");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "attachments");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Domain.Entities.Attachment()
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            FilePath = $"/uploads/attachments/{uniqueFileName}",
            FileSize = file.Length,
            TaskId = request.TaskId,
            UploadedAt = DateTime.UtcNow
        };

        await _attachmentRepo.AddAsync(attachment);
        await _attachmentRepo.SaveChangesAsync();

        return _mapper.Map<AttachmentDto>(attachment);
    }
}