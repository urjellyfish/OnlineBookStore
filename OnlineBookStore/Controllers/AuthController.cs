using Application.Dto;
using Application.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
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
        public IActionResult Register (UserDto userDto)
        {
            var user = _authService.RegisterAsync(userDto).Result;
            if (user == null)
            {
                return BadRequest("User already exist");
            }
            return Ok("Registered Successfully");
        }
    }
}
