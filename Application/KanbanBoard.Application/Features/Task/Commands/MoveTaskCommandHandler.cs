using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;

namespace KanbanBoard.Application.Features.Task.Commands;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, bool>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IColumnRepository _columnRepo;

    public MoveTaskCommandHandler(ITaskRepository taskRepo, IColumnRepository columnRepo)
    {
        _taskRepo = taskRepo;
        _columnRepo = columnRepo;
    }

    public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.MoveTaskDto.TaskId);
        if (task == null)
            throw new Exception("Task not found.");

        var targetColumn = await _columnRepo.GetByIdAsync(request.MoveTaskDto.TargetColumnId);
        if (targetColumn == null)
            throw new Exception("Target column not found.");

        int newOrder;
        if (request.MoveTaskDto.NewOrder.HasValue)
        {
            newOrder = request.MoveTaskDto.NewOrder.Value;
        }
        else
        {
            var maxOrder = await _taskRepo.GetMaxOrderAsync(request.MoveTaskDto.TargetColumnId);
            newOrder = maxOrder + 1;
        }

        task.MoveToColumn(request.MoveTaskDto.TargetColumnId, newOrder);

        _taskRepo.UpdateAsync(task);
        await _taskRepo.SaveChangesAsync();

        return true;
    }
}