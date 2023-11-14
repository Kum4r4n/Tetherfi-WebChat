using Message.Application.Interfaces.Repositories;
using Message.Application.Models;
using Message.Domain.Entities;
using Message.Infrastructure.Context;
using Message.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;

namespace Message.Infrastructure.Repositories
{
    public class UserConnectionInfoRepository : IUserConnectionInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserGrpcProvider _userGrpcProvider;
        public UserConnectionInfoRepository(ApplicationDbContext dbContext, UserGrpcProvider userGrpcProvider)
        {
            _dbContext = dbContext;
            _userGrpcProvider = userGrpcProvider;
        }

        public async Task<bool> AddUpdate(Guid id, string connectionId)
        {
            var isExist = false;
            var userAlreadyExists = _dbContext.UserConnectionInfos.SingleOrDefault(s => s.Id == id);
            if (userAlreadyExists == null)
            {

                var entity = new UserConnectionInfo()
                {
                    Id = id,
                    ConnectionId = connectionId
                };

                await _dbContext.UserConnectionInfos.AddAsync(entity);

            }
            else
            {
                isExist = true;
                userAlreadyExists.ConnectionId = connectionId;
            }

            await _dbContext.SaveChangesAsync();
            return isExist;
        }

        public async Task Remove(Guid id)
        {
            var userAlreadyExists = _dbContext.UserConnectionInfos.SingleOrDefault(s => s.Id == id);
            if (userAlreadyExists != null)
            {
                _dbContext.UserConnectionInfos.Remove(userAlreadyExists);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<List<ChatUserModel>> GetAllUsersExceptThis(Guid id)
        {
            var users = await _dbContext.UserConnectionInfos.Where(w => w.Id != id).Select(s => new ChatUserModel() { Id = s.Id, Name = "", ConnectionId = s.ConnectionId }).ToListAsync();
            var userNames = await _userGrpcProvider.GetUsersNames(users.Select(s => s.Id).ToList());

            foreach (var user in userNames)
            {
                var u = users.SingleOrDefault(s => s.Id == user.Id);
                if (u != null)
                    u.Name = user.Name;
            }
            return users;
        }

        public async Task<ChatUserModel> GetUserData(Guid id)
        {
            var user = await _dbContext.UserConnectionInfos.SingleOrDefaultAsync(s => s.Id == id);
            var userNames = await _userGrpcProvider.GetUsersNames(new List<Guid>() { user.Id });

            var userModel = new ChatUserModel()
            {
                Id = user.Id,
                Name = userNames.FirstOrDefault().Name,
                ConnectionId = user.ConnectionId
            };


            return userModel;
        }

        public async Task<UserConnectionInfo> GetUserInfo(Guid id)
        {
            var user = await _dbContext.UserConnectionInfos.SingleOrDefaultAsync(s => s.Id == id);
            return user;
        }

    }
}
