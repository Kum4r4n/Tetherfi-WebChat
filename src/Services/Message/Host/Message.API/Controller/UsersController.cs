using Message.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Message.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var data = await _userService.GetUsers(User);
            return Ok(data);
        }

        [Authorize]
        [HttpGet("Chatroom/{partnerId}")]
        public async Task<IActionResult> GetChatRoom(Guid partnerId)
        {
            var data = await _userService.GetSepecificChatRoom(partnerId, User);
            return Ok(data);
        }
    }
}
