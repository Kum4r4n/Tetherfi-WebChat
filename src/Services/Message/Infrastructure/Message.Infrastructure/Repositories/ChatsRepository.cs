using Message.Application.Interfaces.Repositories;
using Message.Domain.Entities;
using Message.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Message.Infrastructure.Repositories
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Chat> AddChat(string message, Guid partnerId, Guid userId)
        {
            var exisitingChatRoom = await _dbContext.Chats.Where(x => x.ChatRoomId.Contains(userId.ToString()) && x.ChatRoomId.Contains(partnerId.ToString())).ToListAsync();
            var chatRoomId = exisitingChatRoom?.FirstOrDefault()?.ChatRoomId ?? partnerId + "_" + userId;
            var chatRoom = new Chat()
            {
                ChatRoomId = chatRoomId,
                CreatedDateTime = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Message = message,
                SenderId = userId,
            };

            await _dbContext.Chats.AddAsync(chatRoom);
            await _dbContext.SaveChangesAsync();

            return chatRoom;
        }

        public async Task<List<Chat>> GetChats(Guid userId, Guid partnerId)
        {
            var data = await _dbContext.Chats.Where(x => x.ChatRoomId.Contains(userId.ToString()) && x.ChatRoomId.Contains(partnerId.ToString())).ToListAsync();
            return data;
        }
    }
}
