using Message.Application.Models;
using System.Security.Claims;

namespace Message.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<ChatUserModel>> GetUsers(ClaimsPrincipal user);
        Task<ChatRoomModel> GetSepecificChatRoom(Guid partnerId, ClaimsPrincipal user);
    }
}
