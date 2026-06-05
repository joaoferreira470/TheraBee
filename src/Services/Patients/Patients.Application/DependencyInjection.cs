using BuildingBlocks.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Patients.Application.ProgressDashboards.Services;
using Patients.Application.Reports.Services;
using System.Reflection;

namespace Patients.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IPatientProgressDashboardBuilder, PatientProgressDashboardBuilder>();
        services.AddScoped<IReportDraftBuilder, ReportDraftBuilder>();

        return services;
    }
}
