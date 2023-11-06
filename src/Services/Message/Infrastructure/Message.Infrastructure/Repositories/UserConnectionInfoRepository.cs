using Message.Application.Interfaces.Repositories;
using Message.Domain.Entities;
using Message.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Infrastructure.Repositories
{
    public class UserConnectionInfoRepository : IUserConnectionInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserConnectionInfoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddUpdate(Guid id, string connectionId)
        {
            var isExist = false;
            var userAlreadyExists = _dbContext.UserConnectionInfos.SingleOrDefault(s=>s.Id == id);
            if(userAlreadyExists == null) {

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
            if(userAlreadyExists != null)
            {
                _dbContext.UserConnectionInfos.Remove(userAlreadyExists);
                await _dbContext.SaveChangesAsync();
            }

        }

        public IEnumerable<UserConnectionInfo> GetAllUsersExceptThis(Guid id)
        {
            var users = _dbContext.UserConnectionInfos.Where(w => w.Id != id).AsEnumerable();
            return users;
        }

        public async Task<UserConnectionInfo> GetUserInfo(Guid id)
        {
            var user = await _dbContext.UserConnectionInfos.SingleOrDefaultAsync(s => s.Id == id);
            return user;
        }

    }
}
