using System.ComponentModel.DataAnnotations;

namespace JwtApi.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username majburiy")]
        [MinLength(3, ErrorMessage = "Username kamida 3 ta belgidan iborat bo'lishi kerak")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email majburiy")]
        [EmailAddress(ErrorMessage = "Email formati noto'g'ri")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parol majburiy")]
        [MinLength(6, ErrorMessage = "Parol kamida 6 ta belgidan iborat bo'lishi kerak")]
        public string Password { get; set; } = string.Empty;
    }
}
