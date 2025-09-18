using JwtApi.Models;

namespace JwtApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? GetUserIdFromToken(string token);
    }
}
