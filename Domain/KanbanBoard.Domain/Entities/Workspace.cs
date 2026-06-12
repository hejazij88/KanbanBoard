namespace KanbanBoard.Domain.Models;

public class Workspace
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid OwnerId { get; set; }

    public User Owner { get; set; } = null!;

    public ICollection<Board> Boards { get; set; }
        = new List<Board>();
}