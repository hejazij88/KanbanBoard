using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }
    public string? RefreshToken { get;private set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    private readonly List<WorkspaceMember> _workspaceMembers = new();
    public IReadOnlyCollection<WorkspaceMember> WorkspaceMembers => _workspaceMembers.AsReadOnly();

    private readonly List<TaskItem> _assignedTasks = new();
    public IReadOnlyCollection<TaskItem> AssignedTasks => _assignedTasks.AsReadOnly();

    private readonly List<Comment> _comments = new();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private User()
    {
    }

    public User(
        string username,
        string email,
        string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        Username = username;
    }
    public void SetEmail(string email)
    {
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.");
        Email = email;
    }

    public void SetRefreshToken(string? refreshToken, DateTime? expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void AddWorkspaceMember(WorkspaceMember member)
    {
        if (_workspaceMembers.Any(m => m.UserId == member.UserId && m.WorkspaceId == member.WorkspaceId))
            throw new InvalidOperationException("User is already a member of this workspace.");
        _workspaceMembers.Add(member);
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


}