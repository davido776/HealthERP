using HealthERP.Application.Constants;
using HealthERP.Domain.Identity;
using HealthERP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HealthERP.Presentation.Extensions
{
    public static class IdentityExtensionService
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {

            Services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superdupersecretsuperdupersecretjjgfjdfgudfjgdgfudfudfgudftudgfudftudfgduftudfgduftdufgdufd"));
            Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = key
                };
            });

            Services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.SubmitClaimPolicy, policy =>
                {
                    policy.RequireRole(RoleConstants.PolicyHolderRole);
                });

                options.AddPolicy(PolicyConstants.UpdateClaimPolicy, policy =>
                {
                    policy.RequireRole(RoleConstants.AdministratorRole);
                });
            });

            return Services;
        }
    }
}
