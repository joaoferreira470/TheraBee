using Patients.API;
using Patients.Application;
using Patients.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the container
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

//  Configure the HTTP request pipeline

app.Run();
