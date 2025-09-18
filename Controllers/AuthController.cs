using JwtApi.Models;
using JwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register(RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Ma'lumotlar noto'g'ri",
                        Data = ModelState
                    });
                }

                if (await _userService.UserExistsAsync(request.Email))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Bunday email allaqachon mavjud"
                    });
                }

                var user = await _userService.CreateUserAsync(request);
                var token = _jwtService.GenerateToken(user);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Foydalanuvchi muvaffaqiyatli ro'yxatdan o'tdi",
                    Token = token,
                    Data = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register xatosi");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Server xatosi"
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Ma'lumotlar noto'g'ri",
                        Data = ModelState
                    });
                }

                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user == null || !await _userService.ValidatePasswordAsync(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Email yoki parol noto'g'ri"
                    });
                }

                var token = _jwtService.GenerateToken(user);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Muvaffaqiyatli kirdingiz",
                    Token = token,
                    Data = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login xatosi");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Server xatosi"
                });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Token yaroqsiz"
                    });
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Foydalanuvchi topilmadi"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Profil ma'lumotlari",
                    Data = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile xatosi");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Server xatosi"
                });
            }
        }

        [HttpGet("protected")]
        [Authorize]
        public ActionResult<ApiResponse<object>> GetProtectedData()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Bu himoyalangan ma'lumot!",
                Data = new
                {
                    UserId = userId,
                    UserName = userName,
                    Timestamp = DateTime.UtcNow,
                    Message = "Siz himoyalangan endpointga muvaffaqiyatli kirdingiz!"
                }
            });
        }
    }
}
