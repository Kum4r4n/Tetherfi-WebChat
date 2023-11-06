using Message.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Message.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserConnectionInfo> UserConnectionInfos { get; set; }
    }
}
