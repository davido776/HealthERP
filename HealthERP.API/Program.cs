using HealthERP.Application.Interfaces;
using HealthERP.Domain.Identity;
using HealthERP.Infrasctructure.Security;
using HealthERP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    options.Password.RequireNonAlphanumeric = false;
//})
//.AddEntityFrameworkStores<AppDbContext>();
//.AddSignInManager<SignInManager<ApplicationUser>>()
//.AddDefaultTokenProviders();

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>();


var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "health.db" };
var connectionString = connectionStringBuilder.ToString();
var connection = new SqliteConnection(connectionString);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(connection);
});

//builder.Services.AddScoped<IUserAccessor, UserAccessor>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
