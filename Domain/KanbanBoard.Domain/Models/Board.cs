namespace KanbanBoard.Domain.Models;

public class Board
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid WorkspaceId { get; set; }

    public Workspace Workspace { get; set; } = null!;

    //public ICollection<BoardColumn> Columns { get; set; }
    //    = new List<BoardColumn>();
}