namespace KanbanBoard.Domain.Models;

public class BoardColumn
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public Guid BoardId { get; set; }

    public Board Board { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; set; }
        = new List<TaskItem>();
}