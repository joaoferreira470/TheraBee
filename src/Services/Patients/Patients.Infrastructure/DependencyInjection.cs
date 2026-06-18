using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Application.Data;
using Patients.Application.Services;
using Patients.Infrastructure.AI;
using Patients.Infrastructure.Security;
using Patients.Infrastructure.Storage;

namespace Patients.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //Add services to the container
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.AddInterceptors(new AuditableEntityInterceptor());
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SectionName));
        services.AddHttpClient<IClinicalReportNarrativeGenerator, OpenAiClinicalReportNarrativeGenerator>();
        services.Configure<PortraitStorageOptions>(
            configuration.GetSection(PortraitStorageOptions.SectionName));
        services.AddScoped<IPortraitStorage, LocalPortraitStorage>();

        return services;
    }
}

