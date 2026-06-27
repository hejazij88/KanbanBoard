using System.Security.Claims;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
   
    bool ValidateToken(string token);
}