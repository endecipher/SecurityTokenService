using Core.UserClient.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.UserClient.Data.DB
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ClientUser> ClientUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
