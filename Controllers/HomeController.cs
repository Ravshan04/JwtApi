using JwtApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResponse> Get()
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "JWT API .NET 8 da ishlamoqda!",
                Data = new
                {
                    Version = "1.0.0",
                    Endpoints = new
                    {
                        Register = "POST /api/auth/register",
                        Login = "POST /api/auth/login",
                        Profile = "GET /api/auth/profile (requires Bearer token)",
                        Protected = "GET /api/auth/protected (requires Bearer token)"
                    },
                    ExampleUsage = new
                    {
                        Register = new
                        {
                            Method = "POST",
                            Url = "/api/auth/register",
                            Body = new
                            {
                                Username = "testuser",
                                Email = "test@example.com",
                                Password = "password123"
                            }
                        },
                        Login = new
                        {
                            Method = "POST",
                            Url = "/api/auth/login",
                            Body = new
                            {
                                Email = "test@example.com",
                                Password = "password123"
                            }
                        }
                    }
                }
            });
        }
    }
}
