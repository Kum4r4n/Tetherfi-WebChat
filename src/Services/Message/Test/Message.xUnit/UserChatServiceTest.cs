using Message.Application.Interfaces;
using Message.Application.Interfaces.Repositories;
using Message.Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Message.xUnit
{
    public class UserChatServiceTest
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserConnectionInfoRepository> _mockConnectionInfoRepository = new Mock<IUserConnectionInfoRepository>();
        private readonly Mock<IChatsRepository> _mockChatsRepository = new Mock<IChatsRepository>();

        public UserChatServiceTest()
        {
             _userService = new UserService(_mockConnectionInfoRepository.Object, _mockChatsRepository.Object);
        }

        [Fact]
        public async Task GetSpecificChatRoomTest()
        {
            var partnerId = Guid.Parse("4847EA0A-D1F0-46E3-A18D-EDEC2DF484BF");
            var userId = Guid.Parse("22BDA994-B135-44AE-B23A-3650EB785A05");


            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, "22BDA994-B135-44AE-B23A-3650EB785A05")
            };

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));


            _mockConnectionInfoRepository.Setup(s => s.GetUserData(partnerId)).ReturnsAsync(new Application.Models.ChatUserModel()
            {
                Id = partnerId,
                Name = "test",
                ConnectionId = "1234567890"

            });

            _mockChatsRepository.Setup(s => s.GetChats(userId, partnerId)).ReturnsAsync(new List<Domain.Entities.Chat>()
            {
                new Domain.Entities.Chat()
                {
                    Id = Guid.Parse("A0FA2549-CE71-4466-B824-610C0FC39B76"),
                    ChatRoomId = "chatRoomId",
                    Message = "test",
                    CreatedDateTime = DateTime.UtcNow,
                    SenderId = partnerId
                }
            });

            var result = await _userService.GetSepecificChatRoom(partnerId, userPrincipal);

            Assert.True(result != null && result.ParterId == partnerId && result.Chats != null && result.Chats.Any());
        }

    }
}
