using Message.Application.Interfaces;
using Message.Application.Models;
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

        [HttpPost("image/{partnerId}")]
        public async Task<IActionResult> SendImageMessage(Guid partnerId)
        {
            var file = Request.Form.Files[0];
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();
            await _chatService.SendImageChat(User, partnerId, imageData);
            return Ok();
        }
    }
}
