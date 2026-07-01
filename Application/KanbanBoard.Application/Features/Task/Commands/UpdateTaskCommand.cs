using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Domain.Enums;
using MediatR;
using System.Security.AccessControl;

namespace KanbanBoard.Application.Features.Task.Commands;

public class UpdateTaskCommand : IRequest<TaskDto>
{
    public Guid Id { get; set; }
    public UpdateTaskDto UpdateTaskDto { get; set; }
}