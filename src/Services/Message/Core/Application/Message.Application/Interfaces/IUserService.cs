using Message.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<ChatUserModel>> GetUsers(ClaimsPrincipal user);
        Task<ChatRoomModel> GetSepecificChatRoom(Guid partnerId, ClaimsPrincipal user);
    }
}
