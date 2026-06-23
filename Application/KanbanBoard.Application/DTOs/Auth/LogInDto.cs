namespace KanbanBoard.Application.DTOs.Auth;

public class LogInDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}