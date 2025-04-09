using Patients.API;
using Patients.Application;
using Patients.Infrastructure;
using Patients.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the container
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

//  Configure the HTTP request pipeline

app.UseApiServices();

//If the Environment is Dev the application will auto Migrate upon building
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
