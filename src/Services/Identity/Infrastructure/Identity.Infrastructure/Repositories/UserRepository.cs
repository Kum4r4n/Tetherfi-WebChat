using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Add(User user)
        {
            var data = _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<User> Get(Guid id)
        {
            var data = await _dbContext.Users.SingleOrDefaultAsync(s => s.Id == id);
            return data;
        }

        public async Task<User> Get(string email)
        {
            var data = await _dbContext.Users.SingleOrDefaultAsync(s => s.Email == email);
            return data;
        }

        public async Task<User> Update(User user)
        {
            var data = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return data.Entity;
        }
    }
}
