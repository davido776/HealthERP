using HealthERP.Application.Command.Administrators;
using HealthERP.Application.Interfaces;
using HealthERP.Infrasctructure.Security;
using Microsoft.OpenApi.Models;

namespace HealthERP.Presentation.Extensions
{
    public static class ApplicationExtensionService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services)
        {
            Services.AddScoped<IUserAccessor, UserAccessor>();

            Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthERP.API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                //option.CustomSchemaIds(type => SwashbuckleSchemaHelper.GetSchemaId(type));
            });

            Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAdministrator.Handler).Assembly));

            return Services;

        }
    }
}
