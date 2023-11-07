using Message.Application.Interfaces;
using Message.Application.Interfaces.Repositories;
using Message.Application.Models;
using System.Security.Claims;

namespace Message.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserConnectionInfoRepository _userConnectionInfoRepository;
        private readonly IChatsRepository _chatsRepository;
        public UserService(IUserConnectionInfoRepository userConnectionInfoRepository, IChatsRepository chatsRepository)
        {
            _userConnectionInfoRepository = userConnectionInfoRepository;
            _chatsRepository = chatsRepository;
        }

        public async Task<List<ChatUserModel>> GetUsers(ClaimsPrincipal user)
        {
            var data = await _userConnectionInfoRepository.GetAllUsersExceptThis(Guid.Parse(user.Identity.Name));
            return data;
        }

        public async Task<ChatRoomModel> GetSepecificChatRoom(Guid partnerId, ClaimsPrincipal user)
        {
            var partnerUser = await _userConnectionInfoRepository.GetUserData(partnerId);
            var chats = await _chatsRepository.GetChats(Guid.Parse(user.Identity.Name), partnerId);
            var response = new ChatRoomModel()
            {
                ParterId = partnerId,
                PartnerConnectionId = partnerUser.ConnectionId,
                PartnerName = partnerUser.Name,
                Chats = chats == null ? new List<ChatModel>() : chats.Select(s => new ChatModel()
                {
                    Id = s.Id,
                    ChatRoomId = s.ChatRoomId,
                    Message = s.Message,
                    CreatedDateTime = s.CreatedDateTime,
                    SenderId = s.SenderId,
                }).ToList()
            };

            return response;
        }
    }
}
