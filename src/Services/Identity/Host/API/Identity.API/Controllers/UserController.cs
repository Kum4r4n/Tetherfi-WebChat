using Identity.Application.Interfaces.Services;
using Identity.Application.Models.Request;
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

        
    }
}
