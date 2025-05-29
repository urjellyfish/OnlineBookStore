using Application.Dto;
using Application.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("update/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody]UserDto userDto)
        {
            var success = await _userService.UpdateUserAsync(userId, userDto);

            if (!success) return NotFound("User not found");
            return Ok("User updated successfully");
        }
    }
}
