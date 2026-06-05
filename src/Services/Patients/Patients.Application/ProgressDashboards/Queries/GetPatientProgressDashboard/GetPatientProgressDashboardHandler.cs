namespace Patients.Application.ProgressDashboards.Queries.GetPatientProgressDashboard;

public class GetPatientProgressDashboardHandler(IPatientProgressDashboardBuilder dashboardBuilder, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientProgressDashboardQuery, GetPatientProgressDashboardResult>
{
    public async Task<GetPatientProgressDashboardResult> Handle(GetPatientProgressDashboardQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var dashboard = await dashboardBuilder.BuildAsync(query.PatientId, currentUserId, cancellationToken);

        return new GetPatientProgressDashboardResult(dashboard);
    }
}
