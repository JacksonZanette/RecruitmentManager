using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Interfaces.Services;

namespace RecruitmentManager.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
            => _usersService = usersService;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            await _usersService.CreateAsync(userDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var token = await _usersService.LoginAsync(userDto);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("names")]
        public async Task<IEnumerable<string>> GetNames()
            => await _usersService.GetUserNamesAsync();
    }
}