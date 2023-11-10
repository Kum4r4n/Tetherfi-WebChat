
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Signal.Application.Interfaces.Repository;
using Signal.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signal.Application.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SignalHub : Hub
    {
        private readonly IUserDataRepository _userDataRepository;

        public SignalHub(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        public async Task SendSignal(Message data, Guid partnerId)
        {
            var userData = await _userDataRepository.GetUserData(partnerId);
            var reqUserData = await _userDataRepository.GetUserData(Guid.Parse(Context.User.Identity.Name));
            await Clients.Client(userData.ConnectionId).SendAsync("ReceiveSignal", data, reqUserData);
        }

    }
}
