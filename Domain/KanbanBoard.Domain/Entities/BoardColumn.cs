using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Entities;

public class BoardColumn : AuditableEntity
{
    public string Title { get; private set; }

    public int Order { get; private set; }

    public Guid BoardId { get; private set; }

    public Board Board { get; private set; } = null!;

    private BoardColumn()
    {
    }

    public BoardColumn(string title, Guid boardId, int order)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        BoardId = boardId;
        SetOrder(order);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Column title cannot be empty.");
        Title = title;
    }

    public void SetOrder(int order)
    {
        if (order < 0)
            throw new ArgumentException("Order must be non-negative.");
        Order = order;
    }

    public TaskItem AddTask(string title, string description, Priority priority,
        Guid? assignedUserId = null, DateTime? dueDate = null)
    {
        var maxOrder = _tasks.Any() ? _tasks.Max(t => t.Order) : 0;
        var task = new TaskItem(title, description, priority, Id, maxOrder + 1);
        _tasks.Add(task);

        if (assignedUserId.HasValue)
            task.AssignToUser(assignedUserId.Value);

        if (dueDate.HasValue)
            task.SetDueDate(dueDate.Value);

        return task;
    }

    public void RemoveTask(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new InvalidOperationException("Task not found.");
        _tasks.Remove(task);
    }

    public void ReorderTasks(int fromOrder, int toOrder)
    {
        var sorted = _tasks.OrderBy(t => t.Order).ToList();
        var fromIndex = sorted.FindIndex(t => t.Order == fromOrder);
        var toIndex = sorted.FindIndex(t => t.Order == toOrder);

        if (fromIndex == -1 || toIndex == -1)
            return;

        var item = sorted[fromIndex];
        sorted.RemoveAt(fromIndex);
        sorted.Insert(toIndex, item);

        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].Reorder(i + 1);
        }
    }
    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> TaskItems => _tasks;
}