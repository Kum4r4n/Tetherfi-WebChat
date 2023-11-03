using Identity.Application.Interfaces.Services;
using Identity.Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //didn't handle the exceptions for now
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(RegisterRequestModel registerRequestModel)
        {
            var data = await _userService.RegisterUser(registerRequestModel);
            return Ok(data);
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginRequestModel loginRequestModel)
        {
            var data = await _userService.Login(loginRequestModel);
            return Ok(data);
        }

        [Authorize]
        [HttpPut(nameof(Update))]
        public async Task<IActionResult> Update(UpdateUserRequestModel updateUserRequestModel)
        {
            var data = await _userService.Update(updateUserRequestModel, User);
            return Ok(data);
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserData()
        {
            var data = await _userService.GetUser(User);
            return Ok(data);   
        }
    }
}
