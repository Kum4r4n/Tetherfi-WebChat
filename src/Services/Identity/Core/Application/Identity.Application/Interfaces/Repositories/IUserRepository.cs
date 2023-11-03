using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<User> Update(User user);
        Task<User> Get(string email);
        Task<User> Get(Guid id);
    }
}
