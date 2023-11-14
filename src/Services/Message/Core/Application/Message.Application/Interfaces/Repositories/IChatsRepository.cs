using Message.Domain.Entities;

namespace Message.Application.Interfaces.Repositories
{
    public interface IChatsRepository
    {
        Task<List<Chat>> GetChats(Guid userId, Guid partnerId);
        Task<Chat> AddChat(string message, Guid partnerId, Guid userId);
    }
}
