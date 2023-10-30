using HealthERP.Presentation.Extensions;
using FluentValidation.AspNetCore;
using HealthERP.Application.Command.Administrators;
using FluentValidation;
using HealthERP.Application.Command.PolicyHolders;
using HealthERP.Application.Command.PolicyHolders.Validations;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDatabaseService(configuration);

builder.Services.AddIdentityServices();

builder.Services.AddApplicationService();

builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<CreateAdministrator>();
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

await Initailizer.InitailiseDatabase(app);

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
