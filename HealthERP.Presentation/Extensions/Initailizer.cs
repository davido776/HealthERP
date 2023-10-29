using HealthERP.Domain.Identity;
using HealthERP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Presentation.Extensions
{
    public static class Initailizer
    {
        public static async Task InitailiseDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            //seed db

            var context = Services.GetRequiredService<AppDbContext>();
            var userManager = Services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
            await context.Database.MigrateAsync();
            await Seed.InitializeRoles(roleManager);
            await Seed.SeedData(userManager, roleManager, context);
        }
    }
}
