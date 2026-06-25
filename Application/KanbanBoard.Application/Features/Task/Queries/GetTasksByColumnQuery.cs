using KanbanBoard.Application.DTOs.Task;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Queries;

public class GetTasksByColumnQuery : IRequest<List<TaskDto>>
{
    public Guid ColumnId { get; set; }
}