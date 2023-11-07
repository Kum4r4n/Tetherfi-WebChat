using Message.Application.Hubs;
using Message.Application.Interfaces;
using Message.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly UserChatHub _userChatHub;
        private readonly IUserConnectionInfoRepository _userConnectionInfoRepository;

        public ChatService(IChatsRepository chatsRepository, UserChatHub userChatHub, IUserConnectionInfoRepository userConnectionInfoRepository)
        {
            _chatsRepository = chatsRepository;
            _userChatHub = userChatHub;
            _userConnectionInfoRepository = userConnectionInfoRepository;
        }

        public async Task ReceiveMessage(ClaimsPrincipal user, string message, Guid partnerId)
        {
            var addedMessage = await _chatsRepository.AddChat(message, partnerId, Guid.Parse(user.Identity.Name));
            //partnerId
            var partner = await _userConnectionInfoRepository.GetUserData(partnerId);
            //userId
            var userData = await _userConnectionInfoRepository.GetUserData(Guid.Parse(user.Identity.Name));

            await _userChatHub.ListenMessage(partner.ConnectionId, addedMessage);
            await _userChatHub.ListenMessage(userData.ConnectionId, addedMessage);

        }
    }
}
