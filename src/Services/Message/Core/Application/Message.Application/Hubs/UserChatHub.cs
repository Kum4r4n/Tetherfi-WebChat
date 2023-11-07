using Message.Application.Interfaces.Repositories;
using Message.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Message.Application.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserChatHub : Hub
    {
        private readonly IUserConnectionInfoRepository _userInforrepository;
        private readonly IChatsRepository _chatsRepository;

        public UserChatHub(IUserConnectionInfoRepository userInforrepository, IChatsRepository chatsRepository)
        {
            _userInforrepository = userInforrepository;
            _chatsRepository = chatsRepository;
        }

        public async Task ListenMessage(string connectionId, Chat chat)
        {
            await Clients.Client(connectionId).SendAsync("ListenMessage", chat);
        }

        public async Task SendMessage(string message, Guid partnerId)
        {
            var partnerDetails = await _userInforrepository.GetUserData(partnerId);
            var senderUser = await _userInforrepository.GetUserData(Guid.Parse(Context.User.Identity.Name));
            var chat = await _chatsRepository.AddChat(message, partnerId, Guid.Parse(Context.User.Identity.Name));

            await Clients.Client(partnerDetails.ConnectionId).SendAsync("ListenMessage", chat);
            await Clients.Client(senderUser.ConnectionId).SendAsync("ListenMessage", chat);

        }

        public async Task Join()
        {
            var userExistAndUpdate = await _userInforrepository.AddUpdate(Guid.Parse(Context.User!.Identity!.Name ?? default), Context.ConnectionId);
            var userData = await _userInforrepository.GetUserData(Guid.Parse(Context.User!.Identity!.Name ?? default));
            if (!userExistAndUpdate)//new user
            {
                await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync(
               "NewOnlineUser",
               userData.Name
               );

            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync(
                 "Joined",
                 userData.Name
                );
            }

        }
    }
}
