using AutoMapper;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Application.Features.Column.Queries;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{

    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }



    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(request.TaskId);

        return _mapper.Map<TaskDto>(task.Order);
    }
}