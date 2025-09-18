using JwtApi.Models;

namespace JwtApi.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = new();
        private static int _nextId = 1;

        public Task<User?> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User> CreateUserAsync(RegisterRequest request)
        {
            var user = new User
            {
                Id = _nextId++,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> UserExistsAsync(string email)
        {
            var exists = _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<bool> ValidatePasswordAsync(string password, string hash)
        {
            var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
            return Task.FromResult(isValid);
        }
    }
}
