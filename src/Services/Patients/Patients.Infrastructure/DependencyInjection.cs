using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Application.Data;

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
        return services;
    }
}
