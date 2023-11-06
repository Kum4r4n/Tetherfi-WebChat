using Message.Application.Models;
using Message.Domain.Entities;

namespace Message.Application.Interfaces.Repositories
{
    public interface IUserConnectionInfoRepository
    {
        Task<bool> AddUpdate(Guid id, string connectionId);
        Task Remove(Guid id);
        Task<List<ChatUserModel>> GetAllUsersExceptThis(Guid id);
        Task<UserConnectionInfo> GetUserInfo(Guid id);
    }
}
