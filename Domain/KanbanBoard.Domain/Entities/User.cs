using KanbanBoard.Domain.Common;

public class User : AuditableEntity
{
    public string FullName { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    private User()
    {
    }

    public User(
        string fullName,
        string email,
        string passwordHash)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public ICollection<WorkspaceMember> Workspaces
        = new List<WorkspaceMember>();

    public ICollection<TaskItem> AssignedTasks
        = new List<TaskItem>();
}