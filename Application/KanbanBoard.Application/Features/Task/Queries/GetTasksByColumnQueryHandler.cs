using AutoMapper;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTasksByColumnQueryHandler : IRequestHandler<GetTasksByColumnQuery, List<TaskDto>>
{

    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetTasksByColumnQueryHandler(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskDto>> Handle(GetTasksByColumnQuery request, CancellationToken cancellationToken)
    {
        var listTask = await _taskRepository.GetTasksByColumnAsync(request.ColumnId);

        return _mapper.Map<List<TaskDto>>(listTask);
    }
}