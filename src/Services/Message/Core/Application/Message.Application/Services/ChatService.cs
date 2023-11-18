using Message.Application.Hubs;
using Message.Application.Interfaces;
using Message.Application.Interfaces.Repositories;
using Message.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Message.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly IHubContext<UserChatHub> _userChatHub;
        private readonly IUserConnectionInfoRepository _userConnectionInfoRepository;

        public ChatService(IChatsRepository chatsRepository, IHubContext<UserChatHub> userChatHub, IUserConnectionInfoRepository userConnectionInfoRepository)
        {
            _chatsRepository = chatsRepository;
            _userChatHub = userChatHub;
            _userConnectionInfoRepository = userConnectionInfoRepository;
        }


        //this method not in use
        public async Task ReceiveMessage(ClaimsPrincipal user, string message, Guid partnerId)
        {
            var addedMessage = await _chatsRepository.AddChat(message, partnerId, Guid.Parse(user.Identity.Name));
            //partnerId
            var partner = await _userConnectionInfoRepository.GetUserData(partnerId);
            //userId
            var userData = await _userConnectionInfoRepository.GetUserData(Guid.Parse(user.Identity.Name));

            //await _userChatHub.li(partner.ConnectionId, addedMessage);
            //await _userChatHub.ListenMessage(userData.ConnectionId, addedMessage);

        }

        public async Task SendImageChat(ClaimsPrincipal user, Guid partnerId, byte[] imageBytes)
        {
            var addedMessage = await _chatsRepository.AddImageChat(partnerId, Guid.Parse(user.Identity.Name), imageBytes);
            //partnerId
            var partner = await _userConnectionInfoRepository.GetUserData(partnerId);
            //userId
            var userData = await _userConnectionInfoRepository.GetUserData(Guid.Parse(user.Identity.Name));

            await _userChatHub.Clients.Client(partner.ConnectionId).SendAsync("ListenMessage", addedMessage);
            await _userChatHub.Clients.Client(userData.ConnectionId).SendAsync("ListenMessage", addedMessage);

        }
    }
}
