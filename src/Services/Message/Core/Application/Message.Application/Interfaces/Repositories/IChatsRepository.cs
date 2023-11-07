using Message.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Interfaces.Repositories
{
    public interface IChatsRepository
    {
        Task<List<Chat>> GetChats(Guid userId, Guid partnerId);
        Task<Chat> AddChat(string message, Guid partnerId, Guid userId);
    }
}
