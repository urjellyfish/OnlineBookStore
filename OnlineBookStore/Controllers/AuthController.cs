using Application.Dto;
using Application.Dto.LoginDto;
using Application.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("login-google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // Optionally: check if user exists, register if not, generate JWT etc.
            return Ok(new
            {
                Message = "Login successful with Google",
                Email = email,
                Name = name
            });
        }

    }
}
