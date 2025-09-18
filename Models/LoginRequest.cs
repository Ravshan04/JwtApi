using System.ComponentModel.DataAnnotations;

namespace JwtApi.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email majburiy")]
        [EmailAddress(ErrorMessage = "Email formati noto'g'ri")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parol majburiy")]
        public string Password { get; set; } = string.Empty;
    }
}
