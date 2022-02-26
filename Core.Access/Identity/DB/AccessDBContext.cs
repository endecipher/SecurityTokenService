using Core.Access.Identity.DB.EntitySets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Access.Identity.DB
{
    public class AccessDbContext : IdentityDbContext<IdentityUser>
    {
        public AccessDbContext(DbContextOptions<AccessDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCodeRequest> ClientCodeRequests { get; set; }
        public DbSet<ClientTokenRequest> ClientTokenRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            /// <remarks>
            /// You can type seed data here if you want. For me, I like to create data freshly 
            /// </remarks>
            builder.Entity<Client>().HasData(new Client[]
            {
            });

            builder.Entity<Client>().HasIndex((x) => x.FriendlyName).IsUnique(unique: false).IsClustered(clustered: false);

            base.OnModelCreating(builder);
        }
    }
}