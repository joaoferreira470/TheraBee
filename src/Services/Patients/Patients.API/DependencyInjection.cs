using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Patients.API.Services;
using Patients.Application.Services;
using System.Text;

namespace Patients.API;

public static class DependencyInjection
{
    private const string LocalFrontendCorsPolicy = "LocalFrontend";

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddCors(options =>
        {
            options.AddPolicy(LocalFrontendCorsPolicy, policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200", "http://127.0.0.1:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        services.AddAuthorization();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration["Jwt:SigningKey"]
            ?? throw new InvalidOperationException("Jwt:SigningKey is not configured.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                };
            });

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseCors(LocalFrontendCorsPolicy);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapCarter();

        return app;
    }
}
