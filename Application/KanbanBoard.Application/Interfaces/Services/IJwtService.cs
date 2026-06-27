using System.Security.Claims;

namespace KanbanBoard.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
   
    bool ValidateToken(string token);
}