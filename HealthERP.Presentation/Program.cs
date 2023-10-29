using HealthERP.Application.Command.Administrators;
using HealthERP.Application.Constants;
using HealthERP.Application.Interfaces;
using HealthERP.Domain.Identity;
using HealthERP.Infrasctructure.Security;
using HealthERP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/
// 

var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "health.db" };
var connectionString = connectionStringBuilder.ToString();
var connection = new SqliteConnection(connectionString);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(connection);
});

builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
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
    option.CustomSchemaIds(type => type.ToString());
});


var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superdupersecretsuperdupersecretjjgfjdfgudfjgdgfudfudfgudftudgfudftudfgduftudfgduftdufgdufd"));
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = key
    };
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAdministrator.Handler).Assembly));

builder.Services.AddAuthorization(options =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.


using var scope = app.Services.CreateScope();

var Services = scope.ServiceProvider;

//seed db

var context = Services.GetRequiredService<AppDbContext>();
var userManager = Services.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
await context.Database.MigrateAsync();
await Seed.InitializeRoles(roleManager);
await Seed.SeedData(userManager, roleManager, context);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
