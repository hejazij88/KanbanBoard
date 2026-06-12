using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Models;

public class TaskItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public Priority Priority { get; set; }

    public Guid ColumnId { get; set; }

    public BoardColumn Column { get; set; } = null!;

    public Guid? AssignedUserId { get; set; }

    public User? AssignedUser { get; set; }
}