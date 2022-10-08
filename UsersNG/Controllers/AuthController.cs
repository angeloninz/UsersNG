using Microsoft.AspNetCore.Mvc;
using UsersNG.DTO;
using UsersNG.Models;
using UsersNG.Services.AuthService;

namespace UsersNG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            var response = await _authService.Register(request);

            return response.Success ? Ok(response) : Conflict(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var response = await _authService.Login(request);

            return response.Success ? Ok(response): BadRequest(response);
        }
    }
}
