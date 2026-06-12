namespace KanbanBoard.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string HashPassword { get; set; } = null!;
    public string Email { get; set; } = null!;

}