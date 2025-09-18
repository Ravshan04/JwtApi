using JwtApi.Models;

namespace JwtApi.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(RegisterRequest request);
        Task<bool> UserExistsAsync(string email);
        Task<bool> ValidatePasswordAsync(string password, string hash);
    }
}
