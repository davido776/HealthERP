using HealthERP.Domain.Administrator;
using HealthERP.Domain.Identity;
using HealthERP.Domain.PolicyHolders;
using Microsoft.AspNetCore.Identity;

namespace HealthERP.Persistence
{
    public class Seed
    {
        public static async Task SeedData(
            UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            if (!userManager.Users.Any())
            {
                var adminusers = new List<Administrator>
                {
                    new Administrator
                    {
                        UserName = "admin@test.com",
                        Email = "admin@test.com",
                        FirstName = "bob",
                        LastName = "jones"
                    }
                    
                };

                var holderusers = new List<PolicyHolder>
                {               
                   new PolicyHolder
                    {
                        UserName = "holder@test.com",
                        Email = "holder@test.com",
                        NationalId = "12345",
                        FirstName = "jane",
                        LastName = "doe"
                    }
                };



                foreach (var user in adminusers)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");

                    var newUser = await userManager.FindByEmailAsync(user.Email);

                    var roleExist = await roleManager.RoleExistsAsync("Administrator");

                    if (roleExist)
                    {
                       await userManager.AddToRolesAsync(newUser, new string[] { "Administrator" });
                    }
                    
                }

                foreach (var user in holderusers)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");

                    var newUser = await userManager.FindByEmailAsync(user.Email);

                    var roleExist = await roleManager.RoleExistsAsync("Administrator");

                    if (roleExist)
                    {
                        await userManager.AddToRolesAsync(newUser, new string[] { "PolicyHolder" });
                    }
                }
            }
        }

        public static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrator", "PolicyHolder" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Check if the role already exists
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    // If not, create it
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
