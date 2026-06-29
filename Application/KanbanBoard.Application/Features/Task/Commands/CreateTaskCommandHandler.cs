using AutoMapper;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IColumnRepository _columnRepo;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(ITaskRepository taskRepo, IColumnRepository columnRepo, IMapper mapper)
    {
        _taskRepo = taskRepo;
        _columnRepo = columnRepo;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepo.GetByIdAsync(request.ColumnId);
        if (column == null)
            throw new Exception("Column not found.");

        var maxOrder = await _taskRepo.GetMaxOrderAsync(request.ColumnId);
        var task = column.AddTask(
            request.TaskDto.Title,
            request.TaskDto.Description ?? string.Empty,
            request.TaskDto.Priority,
            request.TaskDto.AssignedUserId,
            request.TaskDto.DueDate
        );

        await _taskRepo.SaveChangesAsync();

        var result = await _taskRepo.GetTaskWithDetailsAsync(task.Id);
        return _mapper.Map<TaskDto>(result);
    }
}