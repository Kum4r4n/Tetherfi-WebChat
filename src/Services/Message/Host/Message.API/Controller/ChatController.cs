using Message.Application.Interfaces;
using Message.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Message.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpPost]
        public async Task<IActionResult> ReceiveMessage(MeesageRequestModel meesageRequestModel)
        {
            await _chatService.ReceiveMessage(User, meesageRequestModel.Message, meesageRequestModel.PartnerId);
            return Ok();
        }
    }
}
