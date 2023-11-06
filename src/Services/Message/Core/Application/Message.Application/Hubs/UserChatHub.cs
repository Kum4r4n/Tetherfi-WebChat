using Message.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserChatHub : Hub
    {
        private readonly IUserConnectionInfoRepository _userInforrepository;

        public UserChatHub(IUserConnectionInfoRepository userInforrepository)
        {
            _userInforrepository = userInforrepository;
        }

        public async Task Leave()
        {
            await _userInforrepository.Remove(Guid.Parse(Context.User!.Identity!.Name ?? default));
            await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync("UserLeft", "User name");
        }

        public async Task Join()
        {
            var userExistAndUpdate = await _userInforrepository.AddUpdate(Guid.Parse(Context.User!.Identity!.Name ?? default), Context.ConnectionId);
            if (!userExistAndUpdate)//new user
            {
                await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync(
               "NewOnlineUser",
               "{New user name}"
               );

            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync(
                 "Joined",
                 "{Joined user name}"
                );
            }
            
            await Clients.Client(Context.ConnectionId).SendAsync(
            "OnlineUsers",
            _userInforrepository.GetAllUsersExceptThis(Guid.Parse(Context.User!.Identity!.Name ?? default))
             );
        }

        public async Task SendDirectMessage(string message, Guid targetUserId)
        {
            var targetUser = await _userInforrepository.GetUserInfo(targetUserId);
            var senderUser = await _userInforrepository.GetUserInfo(Guid.Parse(Context.User!.Identity!.Name ?? default));

            await Clients.Client(targetUser.ConnectionId).SendAsync("SendDM", message, senderUser);
        }
    }
}
