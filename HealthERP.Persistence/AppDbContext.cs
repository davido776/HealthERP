using HealthERP.Domain.Administrator;
using HealthERP.Domain.Claims;
using HealthERP.Domain.Identity;
using HealthERP.Domain.PolicyHolders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PolicyHolder> PolicyHolders { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().UseTptMappingStrategy();
        }
    }
}
