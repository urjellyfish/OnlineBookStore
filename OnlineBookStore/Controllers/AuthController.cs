using Application.Dto;
using Application.Dto.LoginDto;
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
        public async Task<IActionResult> Register (UserDto userDto)
        {
            var user = await _authService.RegisterAsync(userDto);
            if (user == null)
            {
                return BadRequest("User already exist");
            }
            return Ok("Registered Successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            var response = await _authService.Login(loginRequest.Email, loginRequest.Password);
            if (!response.IsSuccess) 
            {
                return Unauthorized(response.Message);
            }
            return Ok(new
            {
                Token = response.Token,
                Email = response.Email,
                FName = response.FName,
                LName = response.LName,
                Role = response.Role
            });
        }
    }
}
